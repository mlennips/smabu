﻿using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Catalogs
{
    public static class GetCatalogItem
    {
        public record GetCatalogItemQuery(CatalogItemId CatalogItemId, CatalogId CatalogId) : IQuery<CatalogItemDTO>;

        public class GetCatalogItemHandler(IAggregateStore store) : IQueryHandler<GetCatalogItemQuery, CatalogItemDTO>
        {
            public async Task<Result<CatalogItemDTO>> Handle(GetCatalogItemQuery request, CancellationToken cancellationToken)
            {
                Catalog catalog = await store.GetByAsync(CatalogId.DefaultId);
                CatalogItem? item = catalog.GetItem(request.CatalogItemId);
                if (item == null)
                {
                    return CatalogErrors.ItemNotFound;
                }
                CatalogGroup groupResult = catalog.GetGroupForItem(item.Id)!;
                return CatalogItemDTO.Create(item, groupResult!);
            }
        }
    }
}
