using LIT.Smabu.Core;
using LIT.Smabu.Infrastructure.Messaging;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LIT.Smabu.Infrastructure.Caching
{
    public class AggregateCache(IServiceProvider serviceProvider) : IAggregateCache,
            IRequestHandler<InformativeNotification.AggregateCreatedEvent>,
            IRequestHandler<InformativeNotification.AggregateUpdatedEvent>,
            IRequestHandler<InformativeNotification.AggregateDeletedEvent>
    {
        readonly static Dictionary<Type, MemoryCache> _cache = [];

        async Task<TAggregate[]> IAggregateCache.GetAllAsync<TAggregate>()
        {
            var allItems = await BrowseAsync<TAggregate>();
            return [.. allItems];
        }

        async Task<TAggregate> IAggregateCache.GetByAsync<TAggregate>(IEntityId<TAggregate> id)
        {
            var cache = await EnsureCacheAsync<TAggregate>();
            var result = cache.Get<TAggregate>(id);
            if (result == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                return result;
            }
        }

        async Task<TAggregate[]> IAggregateCache.GetByAsync<TAggregate>(IEnumerable<IEntityId<TAggregate>> ids)
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

        async Task<TAggregate[]> IAggregateCache.ApplySpecificationTask<TAggregate>(Specification<TAggregate> specification)
        {
            var result = await BrowseAsync(specification);
            return [.. result];
        }

        async Task<int> IAggregateCache.CountAsync<TAggregate>()
        {
            var cache = await EnsureCacheAsync<TAggregate>();
            return cache.Count;
        }

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


        #region RequestHandlers

        public async Task Handle(InformativeNotification.AggregateCreatedEvent request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Aggregate created: {request.Aggregate}");
            var cache = await EnsureCacheAsync(request.Aggregate.GetType());
            if (request.Aggregate is not IAggregateRoot<IEntityId> aggregate)
            {
                throw new InvalidOperationException("Aggregate must implement IAggregateRoot<IEntityId>");
            }
            cache.Set(aggregate.Id, request.Aggregate);
        }

        public async Task Handle(InformativeNotification.AggregateUpdatedEvent request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Aggregate updated: {request.Aggregate}");
            var cache = await EnsureCacheAsync(request.Aggregate.GetType());
            if (request.Aggregate is not IAggregateRoot<IEntityId> aggregate)
            {
                throw new InvalidOperationException("Aggregate must implement IAggregateRoot<IEntityId>");
            }
            cache.Set(aggregate.Id, request.Aggregate);
        }

        public async Task Handle(InformativeNotification.AggregateDeletedEvent request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Aggregate deleted: {request.Aggregate}");
            var cache = await EnsureCacheAsync(request.Aggregate.GetType());
            if (request.Aggregate is not IAggregateRoot<IEntityId> aggregate)
            {
                throw new InvalidOperationException("Aggregate must implement IAggregateRoot<IEntityId>");
            }
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
            if (!_cache.ContainsKey(typeof(TAggregate)))
            {
                _cache[typeof(TAggregate)] = new MemoryCache(new MemoryCacheOptions());
            }

            var cache = _cache[typeof(TAggregate)];
            if (cache.Count == 0)
            {
                var allItems = await LoadFromStoreAsync<TAggregate>();
                foreach (var item in allItems)
                {
                    cache.Set(item.Id, item);
                }
            }

            return cache;
        }

        private async Task<TAggregate[]> LoadFromStoreAsync<TAggregate>() where TAggregate : class, IAggregateRoot<IEntityId<TAggregate>>
        {
            TAggregate[] result = [];
            using (var scope = serviceProvider.CreateScope())
            {
                var store = scope.ServiceProvider.GetRequiredService<IAggregateStore>();
                result = await store.GetAllAsync<TAggregate>();
            }
            return result;
        }

        #endregion
    }
}
