using LIT.Smabu.Infrastructure.Exceptions;
using LIT.Smabu.Infrastructure.Messaging;
using LIT.Smabu.Core;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace LIT.Smabu.Infrastructure.Persistence
{
    public class FileAggregateStore(ILogger<FileAggregateStore> logger, ICurrentUser currentUser, IDomainEventDispatcher domainEventDispatcher) : IAggregateStore
    {
        private readonly string rootDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Smabu", "Data");
        private readonly ILogger<FileAggregateStore> logger = logger;
        private readonly ICurrentUser currentUser = currentUser;
        private readonly IDomainEventDispatcher domainEventDispatcher = domainEventDispatcher;

        public async Task CreateAsync<TAggregate>(TAggregate aggregate)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            aggregate.UpdateMeta(new AggregateMeta(1, DateTime.Now, currentUser.Username, currentUser.Name, null, null, null));
            await SaveToFileAsync(aggregate);

            await domainEventDispatcher.PublishInformativeCreatedNotificationAsync(aggregate);
            await domainEventDispatcher.HandleDomainEventsAsync(aggregate);
        }

        public async Task UpdateAsync<TAggregate>(TAggregate aggregate)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var latestVersion = await GetByAsync(aggregate.Id);
            if (latestVersion != null)
            {
                var meta = aggregate.Meta;
                if (meta != null)
                {
                    aggregate.UpdateMeta(new AggregateMeta(meta.Version + 1, meta.CreatedAt, meta.CreatedById, meta.CreatedByName,
                        DateTime.Now, currentUser.Username, currentUser.Name));
                }
                else
                {
                    throw new NotImplementedException("No meta");
                }
            }
            else
            {
                aggregate.UpdateMeta(new AggregateMeta(1, DateTime.Now, currentUser.Username, currentUser.Name, null, null, null));
            }
            await SaveToFileAsync(aggregate);

            await domainEventDispatcher.PublishInformativeUpdatedNotificationEventAsync(aggregate);
            await domainEventDispatcher.HandleDomainEventsAsync(aggregate);
        }

        public async Task<TAggregate[]> GetAllAsync<TAggregate>()
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            return await LoadAsync<TAggregate>();
        }

        public async Task<TAggregate> GetByAsync<TAggregate>(IEntityId<TAggregate> id)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var result = (await LoadAsync<TAggregate>([id])).FirstOrDefault();
            return result ?? throw new AggregateNotFoundException(id);
        }

        public async Task<TAggregate[]> GetByAsync<TAggregate>(IEnumerable<IEntityId<TAggregate>> ids)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var allItems = await LoadAsync<TAggregate>(ids);
            return allItems.Where(x => ids.Contains(x.Id)).ToArray();
        }

        public async Task DeleteAsync<TAggregate>(TAggregate aggregate)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var file = GetFilePath(aggregate);
            File.Delete(file);

            await domainEventDispatcher.PublishInformativeDeletedNotificationAsync(aggregate);
            await domainEventDispatcher.HandleDomainEventsAsync(aggregate);
        }

        public async Task<int> CountAsync<TAggregate>()
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var items = await this.GetAllAsync<TAggregate>();
            return items.Length;
        }

        public async Task<TAggregate[]> ApplySpecificationTask<TAggregate>(Specification<TAggregate> specification)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            IQueryable<TAggregate> queryable = (await this.GetAllAsync<TAggregate>()).AsQueryable();
            return [.. Specifications.SpecificationEvaluator.GetQuery(queryable, specification)];
        }

        private async Task SaveToFileAsync<TAggregate>(TAggregate aggregate)
            where TAggregate : IAggregateRoot<IEntityId<TAggregate>>
        {
            string path = GetFilePath(aggregate);
            var json = AggregateJsonConverter.ConvertToJson(aggregate);
            await File.WriteAllTextAsync(path, json);
        }

        private async Task<TAggregate[]> LoadAsync<TAggregate>(IEnumerable<IEntityId>? ids = null)
        {
            var result = new List<TAggregate>();
            var aggregateType = typeof(TAggregate);
            string directory = BuildDirectoryPath(aggregateType);
            var fileNames = Directory.GetFiles(directory, "*.json", SearchOption.TopDirectoryOnly);
            if (ids != null)
            {
                fileNames = fileNames.Where(x => ids.Any(y => x.Contains(y.Value.ToString()))).ToArray();
            }
            logger.LogInformation("Read {files} files for type {aggregate} ", fileNames.Length, aggregateType.Name);
            foreach (var fileName in fileNames)
            {
                var fileContent = await File.ReadAllTextAsync(fileName);
                var aggregate = AggregateJsonConverter.ConvertFromJson(aggregateType, fileContent);
                if (aggregate != null && aggregate is TAggregate aggregateWithId)
                {
                    result.Add(aggregateWithId);
                }
                else
                {
                    logger.LogWarning("Invalid aggregate: {fileName}", fileName);
                }
            }
            return [.. result];
        }

        private string GetFilePath<TEntityId>(IAggregateRoot<TEntityId> aggregate) where TEntityId : class, IEntityId
        {
            string directory = BuildDirectoryPath(aggregate.GetType());
            var path = Path.Combine(directory, aggregate.Id.Value.ToString() + ".json");
            return path;
        }

        private string BuildDirectoryPath(Type aggregateType)
        {
            var typeName = aggregateType.Name;
            var directory = Path.Combine(rootDirectory, typeName);
            if (!Directory.Exists(directory))
            {
                logger.LogDebug("Directory created {directory}", directory);
                Directory.CreateDirectory(directory);
            }
            return directory;
        }
    }
}
