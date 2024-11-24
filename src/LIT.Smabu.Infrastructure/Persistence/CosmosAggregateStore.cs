using LIT.Smabu.Infrastructure.Exceptions;
using LIT.Smabu.Infrastructure.Messaging;
using LIT.Smabu.Core;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace LIT.Smabu.Infrastructure.Persistence
{
    public class CosmosAggregateStore(ICurrentUser currentUser, IConfiguration config,
            ILogger<CosmosAggregateStore> logger, IDomainEventDispatcher domainEventDispatcher) : IAggregateStore
    {
        private const string AggregatesContainerId = "Aggregates";
        private static Container? container;
        private readonly ICurrentUser currentUser = currentUser;

        public async Task CreateAsync<TAggregate>(TAggregate aggregate)
             where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            if (aggregate.Meta == null)
            {
                aggregate.UpdateMeta(AggregateMeta.CreateFirst(currentUser));
            }
            var container = await GetAggregatesContainerAsync();
            var cosmosEntity = CreateCosmosEntity(aggregate);
            var response = await container.CreateItemAsync(cosmosEntity, new PartitionKey(cosmosEntity.PartitionKey));
            ValidateResponse(response.StatusCode, HttpStatusCode.Created, "Creation", aggregate);
            logger.LogDebug("Created aggregate {type}/{id} successfully", typeof(TAggregate).Name, aggregate.Id);

            await domainEventDispatcher.PublishInformativeCreatedNotificationAsync(aggregate);
            await domainEventDispatcher.HandleDomainEventsAsync(aggregate);
        }

        public async Task UpdateAsync<TAggregate>(TAggregate aggregate)
             where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            aggregate.UpdateMeta(aggregate.Meta!.Next(currentUser));
            var container = await GetAggregatesContainerAsync();
            var cosmosEntity = CreateCosmosEntity(aggregate);
            var response = await container.ReplaceItemAsync(cosmosEntity, cosmosEntity.Id, new PartitionKey(cosmosEntity.PartitionKey));
            ValidateResponse(response.StatusCode, HttpStatusCode.OK, "Updating", aggregate);
            logger.LogDebug("Updated aggregate {type}/{id} successfully", typeof(TAggregate).Name, aggregate.Id);

            await domainEventDispatcher.PublishInformativeUpdatedNotificationEventAsync(aggregate);
            await domainEventDispatcher.HandleDomainEventsAsync(aggregate);
        }

        public async Task DeleteAsync<TAggregate>(TAggregate aggregate)
             where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var container = await GetAggregatesContainerAsync();
            var cosmosEntity = CreateCosmosEntity(aggregate);
            var response = await container.DeleteItemAsync<TAggregate>(cosmosEntity.Id, new PartitionKey(cosmosEntity.PartitionKey));
            ValidateResponse(response.StatusCode, HttpStatusCode.NoContent, "Deleting", aggregate);
            logger.LogDebug("Deleted aggregate {type}/{id} successfully", typeof(TAggregate).Name, aggregate.Id);

            await domainEventDispatcher.PublishInformativeDeletedNotificationAsync(aggregate);
            await domainEventDispatcher.HandleDomainEventsAsync(aggregate);
        }

        public async Task<TAggregate[]> GetAllAsync<TAggregate>()
             where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var container = await GetAggregatesContainerAsync();
            var sqlQueryText = $"SELECT * FROM c WHERE c.partitionKey = '{GetPartitionKey<TAggregate>()}'";
            var result = await ExecuteQueryAsync<TAggregate>(container, sqlQueryText);
            logger.LogDebug("Get all aggregates of type {type}: {count} items", typeof(TAggregate).Name, result.Length);
            return result;
        }

        public async Task<TAggregate> GetByAsync<TAggregate>(IEntityId<TAggregate> id)
             where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var container = await GetAggregatesContainerAsync();
            var sqlQueryText = $"SELECT TOP 1 * FROM c WHERE c.partitionKey = '{GetPartitionKey<TAggregate>()}' AND c.id = '{id.Value}'";
            var result = await ExecuteQueryAsync<TAggregate>(container, sqlQueryText);
            logger.LogDebug("Get aggregate {type}/{id}", typeof(TAggregate).Name, id);
            return result != null ? result.Single() : throw new AggregateNotFoundException(id);
        }

        public async Task<TAggregate[]> GetByAsync<TAggregate>(IEnumerable<IEntityId<TAggregate>> ids)
             where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            if (!ids.Any()) return [];

            var container = await GetAggregatesContainerAsync();
            var formattedIds = ids.Distinct().Select(x => $"\"{x.Value}\"").ToList();
            var sqlQueryText = $"SELECT * FROM c WHERE c.partitionKey = '{GetPartitionKey<TAggregate>()}' AND c.id IN ({string.Join(',', formattedIds)})";
            var result = await ExecuteQueryAsync<TAggregate>(container, sqlQueryText);
            logger.LogDebug("Get aggregates of type {type} by ids. Found {found}/{requested}",
                typeof(TAggregate).Name, result.Length, ids.Distinct().Count());
            return result;
        }

        public async Task<int> CountAsync<TAggregate>()
             where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var container = await GetAggregatesContainerAsync();
            var result = await container.GetItemLinqQueryable<CosmosEntity<TAggregate>>()
                .Where(x => x.PartitionKey == GetPartitionKey<TAggregate>())
                .Select(x => x.Body).CountAsync();

            logger.LogDebug("Count aggregate {type}", typeof(TAggregate).Name);
            return result;
        }

        public async Task<TAggregate[]> ApplySpecificationTask<TAggregate>(Specification<TAggregate> specification)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var container = await GetAggregatesContainerAsync();
            var partitionKey = GetPartitionKey<TAggregate>();
            IQueryable<CosmosEntity<TAggregate>> queryable = container.GetItemLinqQueryable<CosmosEntity<TAggregate>>()
                .Where(x => x.PartitionKey == partitionKey);
            var specificQueryable = Specifications.SpecificationEvaluator.GetQuery(queryable.Select(x => x.Body), specification);
            var result = await ExecuteQueryAsync(specificQueryable);
            logger.LogDebug("Get aggregates of type {type} by specification '{specification}': {items} items", typeof(TAggregate).Name, specification.GetType().Name, result.Length);
            return result;
        }

        private async Task<Container> GetAggregatesContainerAsync()
        {
            if (container == null)
            {
                var endpointUri = config["AzureAD:Cosmos:Endpoint"];
                var primaryKey = config["AzureAD:Cosmos:PrimaryKey"];
                var databaseId = config["AzureAD:Cosmos:DatabaseId"];
                var cosmosClient = new CosmosClient(endpointUri, primaryKey);
                var databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
                var database = databaseResponse.Database;
                var response = await database!.CreateContainerIfNotExistsAsync(AggregatesContainerId, "/partitionKey");
                container = response.Container;
            }
            return container;
        }

        private static string GetPartitionKey<TAggregate>() where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            return typeof(TAggregate).Name;
        }

        private static CosmosEntity<T> CreateCosmosEntity<T>(T aggregate) where T : class, IAggregateRoot<IEntityId<T>>
        {
            return new CosmosEntity<T>(aggregate.Id.Value.ToString(), aggregate, GetPartitionKey<T>());
        }

        private static async Task<TAggregate[]> ExecuteQueryAsync<TAggregate>(Container container, string sqlQueryText)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            QueryDefinition queryDefinition = new(sqlQueryText);
            var queryResultSetIterator = container.GetItemQueryIterator<CosmosEntity<TAggregate>>(queryDefinition);

            List<TAggregate> result = [];
            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                result.AddRange(currentResultSet.Select(item => item.Body));
            }
            return [.. result];
        }

        private static async Task<TAggregate[]> ExecuteQueryAsync<TAggregate>(IQueryable<TAggregate> queryable)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var setIterator = queryable.ToFeedIterator();
            List<TAggregate> result = [];
            while (setIterator.HasMoreResults)
            {
                result.AddRange(await setIterator.ReadNextAsync());
            }
            return [.. result];
        }

        private void ValidateResponse<TAggregate>(HttpStatusCode responseStatusCode, HttpStatusCode expectedStatusCode, string process, TAggregate aggregate)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            if (responseStatusCode != expectedStatusCode)
            {
                logger.LogError("Operation {process} on aggregate {AggregateType}/{AggregateId} failed with code: {StatusCode}", 
                    process, typeof(TAggregate).Name, aggregate.Id, responseStatusCode);
                throw new SmabuException($"Operation on aggregate '{aggregate.Id}' failed with code: {responseStatusCode}");
            }
        }

        public record CosmosEntity<T>
        {
            [JsonConstructor]
            public CosmosEntity(string id, T body, string partitionKey)
            {
                Id = id;
                Body = body;
                PartitionKey = partitionKey;
            }

            [JsonProperty("id")]
            public string Id { get; init; }

            [JsonProperty("body")]
            public T Body { get; init; }

            [JsonProperty("partitionKey")]
            public string PartitionKey { get; init; }
        }
    }
}
