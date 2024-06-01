﻿namespace LIT.Smabu.Domain.SeedWork
{
    public interface IAggregateRoot<out TEntityId> : IAggregateRoot, IEntity<TEntityId>
        where TEntityId : class, IEntityId
    {

    }

    public interface IAggregateRoot : IEntity
    {
        public new AggregateMeta? Meta { get; }
        void UpdateMeta(AggregateMeta aggregateMeta);
    }
}

