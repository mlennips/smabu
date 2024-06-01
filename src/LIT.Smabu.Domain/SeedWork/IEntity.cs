﻿namespace LIT.Smabu.Domain.SeedWork
{
    public interface IEntity<out TEntityId> : IEntity where TEntityId : IEntityId
    {
        public TEntityId Id { get; }
    }

    public interface IEntity
    {
        public EntityMeta? Meta { get; }
    }
}

