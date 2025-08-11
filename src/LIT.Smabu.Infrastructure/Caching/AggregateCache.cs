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
        Persistence.IAggregateStoreFactory aggregateStoreFactory) : IAggregateCache,
            IRequestHandler<InformativeNotification.AggregateCreatedEvent>,
            IRequestHandler<InformativeNotification.AggregateUpdatedEvent>,
            IRequestHandler<InformativeNotification.AggregateDeletedEvent>
    {
        readonly static Dictionary<Type, MemoryCache> _cache = [];

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
            var result = cache.Get<TAggregate>(id);
            return result ?? throw new InvalidOperationException($"Aggregate with Id {id.Value} not found");
        }

        public async Task<TAggregate[]> GetByAsync<TAggregate>(IEnumerable<IEntityId<TAggregate>> ids)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var cache = await EnsureCacheAsync<TAggregate>();
            var result = new List<TAggregate>();
            foreach (var id in ids.Distinct())
            {
                var entry = cache.Get<TAggregate>(id);
                if (entry != null)
                {
                    result.Add(entry);
                }
            }
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

        public async Task Handle(InformativeNotification.AggregateCreatedEvent request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Aggregate created: {aggregate}", request.Aggregate);
            if (request.Aggregate is not IAggregateRoot<IEntityId> aggregate)
            {
                throw new InvalidOperationException("Aggregate must implement IAggregateRoot<IEntityId>");
            }
            var cache = await EnsureCacheAsync(request.Aggregate.GetType());
            cache.Set(aggregate.Id, request.Aggregate);
        }

        public async Task Handle(InformativeNotification.AggregateUpdatedEvent request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Aggregate updated: {aggregate}", request.Aggregate);
            if (request.Aggregate is not IAggregateRoot<IEntityId> aggregate)
            {
                throw new InvalidOperationException("Aggregate must implement IAggregateRoot<IEntityId>");
            }
            var cache = await EnsureCacheAsync(request.Aggregate.GetType());
            cache.Set(aggregate.Id, request.Aggregate);
        }

        public async Task Handle(InformativeNotification.AggregateDeletedEvent request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Aggregate deleted: {aggregate}", request.Aggregate);
            if (request.Aggregate is not IAggregateRoot<IEntityId> aggregate)
            {
                throw new InvalidOperationException("Aggregate must implement IAggregateRoot<IEntityId>");
            }
            var cache = await EnsureCacheAsync(request.Aggregate.GetType());
            cache.Remove(aggregate.Id);
        }

        #endregion

        #region CacheHandling

        private async Task<MemoryCache> EnsureCacheAsync(Type aggregateType)
        {
            var method = typeof(AggregateCache).GetMethod(nameof(EnsureCacheAsync), BindingFlags.NonPublic | BindingFlags.Instance, []);
            var methodGeneric = method!.MakeGenericMethod(aggregateType);
            var result = await (Task<MemoryCache>)methodGeneric!.Invoke(this, null)!;
            return result;
        }

        private async Task<MemoryCache> EnsureCacheAsync<TAggregate>()
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            MemoryCache? cache;
            if (!_cache.ContainsKey(typeof(TAggregate)))
            {
                cache = new MemoryCache(new MemoryCacheOptions());
                _cache[typeof(TAggregate)] = cache;
                var allItems = await LoadFromStoreAsync<TAggregate>();
                foreach (var item in allItems)
                {
                    cache.Set(item.Id, item);
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
            var store = aggregateStoreFactory.Create();
            var result = await store.GetAllAsync<TAggregate>();
            return result;
        }

        #endregion

        #region Private

        private async Task<IEnumerable<TAggregate>> BrowseAsync<TAggregate>(Specification<TAggregate>? specification = null)
            where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            var cache = await EnsureCacheAsync<TAggregate>();
            var values = new List<TAggregate>();

            foreach (var entryKey in cache.Keys)
            {
                var entry = cache.Get<TAggregate>(entryKey);
                if (entry != null)
                {
                    values.Add(entry);
                }
                else
                {
                    //
                }
            }
            if (specification != null)
            {
                values = [.. Specifications.SpecificationEvaluator.GetQuery(values.AsQueryable(), specification)];
            }
            return values;
        }
        #endregion
    }
}
