using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Infrastructure.Messaging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LIT.Smabu.Domain.Services.GetSalesByYear;

namespace LIT.Smabu.Infrastructure.Persistence
{
    public class UnitOfWork(IAggregateRepository aggregateRepository, IDomainEventDispatcher domainEventDispatcher) : IUnitOfWork
    {
        public IAggregateRepository Repository { get; } = new AggregateRepositoryWrapper(aggregateRepository);

        public bool HasChanges => ((AggregateRepositoryWrapper)Repository).GetChanges().Count > 0;

        public async Task CommitAsync(CancellationToken? cancellationToken = null)
        {
            var repository = (AggregateRepositoryWrapper)Repository;
            var changes = repository.GetChanges();

            if (cancellationToken == null || !cancellationToken.Value.IsCancellationRequested)
            {
                ValidateAggregates(changes);
            }
            if (cancellationToken == null || !cancellationToken.Value.IsCancellationRequested)
            {
                await SaveAggregatesAsync(changes);
                await PublishEventsAsync(changes);
            }
        }

        private static void ValidateAggregates(IReadOnlyList<(ChangeReason Reason, IAggregateRoot Aggregate, Task Action)> changes)
        {
            var validationResults = changes.Select(item => item.Aggregate.Validate()).ToList();
            if (validationResults.Any(result => result.IsFailure))
            {
                var errorMessages = string.Join("; ", validationResults.Where(result => result.IsFailure).Select(result => result.Error));
                throw new DomainException($"Validation failed: {errorMessages}");
            }
        }

        private static async Task SaveAggregatesAsync(IReadOnlyList<(ChangeReason Reason, IAggregateRoot Aggregate, Task Action)> changes)
        {
            foreach (var item in changes)
            {
                switch (item.Reason)
                {
                    case ChangeReason.Created:
                        await item.Action;
                        break;
                    case ChangeReason.Updated:
                        await item.Action;
                        break;
                    case ChangeReason.Deleted:
                        await item.Action;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(changes), item.Reason, null);
                }
            }
        }

        private async Task PublishEventsAsync(IReadOnlyList<(ChangeReason Reason, IAggregateRoot Aggregate, Task Action)> changes)
        {
            var aggregates = changes.Select(item => item.Aggregate).DistinctBy(x => x.DisplayId).ToList();
            List<DomainEventBase> uncommittedEvents = [];
            foreach (var aggregate in aggregates)
            {
                uncommittedEvents.AddRange(aggregate.GetUncommittedEvents(true));
            }
            await domainEventDispatcher.HandleDomainEventsAsync([.. uncommittedEvents]);

            foreach (var item in changes)
            {
                switch (item.Reason)
                {
                    case ChangeReason.Created:
                        await domainEventDispatcher.PublishInformativeCreatedNotificationAsync(item.Aggregate);
                        break;
                    case ChangeReason.Updated:
                        await domainEventDispatcher.PublishInformativeUpdatedNotificationEventAsync(item.Aggregate);
                        break;
                    case ChangeReason.Deleted:
                        await domainEventDispatcher.PublishInformativeDeletedNotificationAsync(item.Aggregate);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(changes), item.Reason, null);
                }
            }
        }

        private class AggregateRepositoryWrapper(IAggregateRepository repository) : IAggregateRepository
        {
            readonly List<(ChangeReason Reason, IAggregateRoot Aggregate, Task Action)> changes = [];

            public IReadOnlyList<(ChangeReason Reason, IAggregateRoot Aggregate, Task Action)> GetChanges() => changes;

            Task IAggregateRepository.CreateAsync<TAggregate>(TAggregate aggregate)
            {
                ArgumentNullException.ThrowIfNull(aggregate);

                changes.Add((ChangeReason.Created, aggregate, repository.CreateAsync(aggregate)));
                return Task.CompletedTask;
            }

            Task IAggregateRepository.UpdateAsync<TAggregate>(TAggregate aggregate)
            {
                ArgumentNullException.ThrowIfNull(aggregate);

                changes.Add((ChangeReason.Updated, aggregate, repository.UpdateAsync(aggregate)));
                return Task.CompletedTask;
            }

            Task IAggregateRepository.DeleteAsync<TAggregate>(TAggregate aggregate)
            {
                ArgumentNullException.ThrowIfNull(aggregate);

                changes.Add((ChangeReason.Deleted, aggregate, repository.DeleteAsync(aggregate)));
                return Task.CompletedTask;
            }

            Task<int> IAggregateRepository.CountAsync<TAggregate>()
                => repository.CountAsync<TAggregate>();

            Task<TAggregate[]> IAggregateRepository.GetAllAsync<TAggregate>()
                => repository.GetAllAsync<TAggregate>();

            Task<TAggregate> IAggregateRepository.GetByAsync<TAggregate>(IEntityId<TAggregate> id)
                => repository.GetByAsync(id);

            Task<TAggregate[]> IAggregateRepository.GetByAsync<TAggregate>(IEnumerable<IEntityId<TAggregate>> ids)
                => repository.GetByAsync(ids);

            Task<TAggregate[]> IAggregateRepository.ApplySpecificationTask<TAggregate>(Specification<TAggregate> specification) 
                => repository.ApplySpecificationTask(specification);
        }

        private enum ChangeReason
        {
            Created,
            Updated,
            Deleted
        }
    }
}
