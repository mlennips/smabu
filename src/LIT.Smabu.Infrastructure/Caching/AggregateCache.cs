using LIT.Smabu.Core;
using LIT.Smabu.Infrastructure.Messaging;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LIT.Smabu.Infrastructure.Caching
{
    public class AggregateCache(ILogger<AggregateCache> logger,
        Persistence.IAggregateRepositoryFactory aggregateStoreFactory) : IAggregateCache,
            IRequestHandler<InformativeNotification.AggregateCreatedEvent, VoidResult>,
            IRequestHandler<InformativeNotification.AggregateUpdatedEvent, VoidResult>,
            IRequestHandler<InformativeNotification.AggregateDeletedEvent, VoidResult>
    {
        readonly static Dictionary<Type, Dictionary<Guid, IAggregateRoot>> _cache = [];

        public async Task<TAggregate[]> GetAllAsync<TAggregate>()
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var allItems = await BrowseAsync<TAggregate>();
            return [.. allItems];
        }

        public async Task<TAggregate> GetByAsync<TAggregate>(IEntityId<TAggregate> id)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var cache = await EnsureCacheAsync<TAggregate>();
            var result = cache.TryGetValue(id.Value, out IAggregateRoot? value)
                ? value as TAggregate 
                : null;
            return result ?? throw new InvalidOperationException($"Aggregate with Id {id.Value} not found");
        }

        public async Task<TAggregate[]> GetByAsync<TAggregate>(IEnumerable<IEntityId<TAggregate>> ids)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var cache = await EnsureCacheAsync<TAggregate>();
            var result = cache.Values.OfType<TAggregate>()
                .Where(x => ids.Contains(x.Id));
            return [.. result];
        }

        public async Task<TAggregate[]> ApplySpecificationTask<TAggregate>(Specification<TAggregate> specification)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var result = await BrowseAsync(specification);
            return [.. result];
        }

        public async Task<int> CountAsync<TAggregate>()
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var cache = await EnsureCacheAsync<TAggregate>();
            return cache.Count;
        }

        #region RequestHandlers

        public async Task<VoidResult> Handle(InformativeNotification.AggregateCreatedEvent request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Aggregate created: {aggregate}", request.Aggregate);
            if (request.Aggregate is not IAggregateRoot<IEntityId> aggregate)
            {
                throw new InvalidOperationException("Aggregate must implement IAggregateRoot<IEntityId>");
            }
            var cache = await EnsureCacheAsync(request.Aggregate.GetType());
            cache.Add(aggregate.Id.Value, request.Aggregate);
            return Result.Void();
        }

        public async Task<VoidResult> Handle(InformativeNotification.AggregateUpdatedEvent request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Aggregate updated: {aggregate}", request.Aggregate);
            if (request.Aggregate is not IAggregateRoot<IEntityId> aggregate)
            {
                throw new InvalidOperationException("Aggregate must implement IAggregateRoot<IEntityId>");
            }
            var cache = await EnsureCacheAsync(request.Aggregate.GetType());
            cache[aggregate.Id.Value] = request.Aggregate;
            return Result.Void();
        }

        public async Task<VoidResult> Handle(InformativeNotification.AggregateDeletedEvent request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Aggregate deleted: {aggregate}", request.Aggregate);
            if (request.Aggregate is not IAggregateRoot<IEntityId> aggregate)
            {
                throw new InvalidOperationException("Aggregate must implement IAggregateRoot<IEntityId>");
            }
            var cache = await EnsureCacheAsync(request.Aggregate.GetType());
            cache.Remove(aggregate.Id.Value);
            return Result.Void();
        }

        #endregion

        #region CacheHandling

        private async Task<Dictionary<Guid, IAggregateRoot>> EnsureCacheAsync(Type aggregateType)
        {
            var method = typeof(AggregateCache).GetMethod(nameof(EnsureCacheAsync), BindingFlags.NonPublic | BindingFlags.Instance, []);
            var methodGeneric = method!.MakeGenericMethod(aggregateType);
            var result = await (Task<Dictionary<Guid, IAggregateRoot>>)methodGeneric!.Invoke(this, null)!;
            return result;
        }

        private async Task<Dictionary<Guid, IAggregateRoot>> EnsureCacheAsync<TAggregate>()
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            Dictionary<Guid, IAggregateRoot>? cache;
            if (!_cache.ContainsKey(typeof(TAggregate)))
            {
                logger.LogInformation("Creating cache for type {type}", typeof(TAggregate).Name);
                cache = [];
                _cache[typeof(TAggregate)] = cache;
                var allItems = await LoadFromStoreAsync<TAggregate>();
                foreach (var item in allItems)
                {
                    cache.Add(item.Id.Value, item);
                }
            }
            else
            {
                cache = _cache[typeof(TAggregate)];
            }
            if (cache == null)
            {
                throw new InvalidOperationException($"Cache for type {typeof(TAggregate).Name} could not be created.");
            }
            return cache;
        }

        private async Task<TAggregate[]> LoadFromStoreAsync<TAggregate>() where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var repository = aggregateStoreFactory.Create();
            var result = await repository.GetAllAsync<TAggregate>();
            return result;
        }

        #endregion

        #region Private

        private async Task<IEnumerable<TAggregate>> BrowseAsync<TAggregate>(Specification<TAggregate>? specification = null)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var cache = await EnsureCacheAsync<TAggregate>();
            var values = cache.Values.OfType<TAggregate>().AsQueryable();
            TAggregate[]? result;
            if (specification != null)
            {
                result = [.. Specifications.SpecificationEvaluator.GetQuery(values, specification)];
            }
            else
            {
                result = [.. values];
            }
            return result;
        }
        #endregion
    }
}
