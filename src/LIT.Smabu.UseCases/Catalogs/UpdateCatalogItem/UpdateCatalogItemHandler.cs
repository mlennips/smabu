﻿using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Domain.Shared;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Catalogs.UpdateCatalogItem
{
    public class UpdateCatalogItemHandler(IAggregateStore store) : ICommandHandler<UpdateCatalogItemCommand>
    {
        public async Task<Result> Handle(UpdateCatalogItemCommand request, CancellationToken cancellationToken)
        {
            var catalog = await store.GetByAsync(CatalogId.DefaultId);
            var updateResult = catalog.UpdateItem(request.Id, request.Name, request.Description, request.IsActive,
                request.Unit, request.Prices);
            await store.UpdateAsync(catalog);
            return updateResult;
        }
    }
}