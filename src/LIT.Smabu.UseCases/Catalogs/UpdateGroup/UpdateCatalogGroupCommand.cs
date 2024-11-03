﻿using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Catalogs.UpdateGroup
{
    public record UpdateCatalogGroupCommand(CatalogId CatalogId, CatalogGroupId Id, string Name, string Description) : ICommand
    {

    }
}