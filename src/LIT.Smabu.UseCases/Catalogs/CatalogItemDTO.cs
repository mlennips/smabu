﻿using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Catalogs
{
    public record CatalogItemDTO(CatalogItemId Id, CatalogGroupId CatalogGroupId, CatalogId CatalogId, string DisplayName, CatalogItemNumber Number, string Name,
        string Description, bool IsActive, string GroupName,
        CatalogItemPrice[] Prices, CustomerCatalogItemPrice[] CustomerPrices, Unit Unit, CatalogItemPrice CurrentPrice) : IDTO
    {
        public Currency Currency { get; } = Currency.EUR;

        public static CatalogItemDTO Create(CatalogItem item, CatalogGroup catalogGroup)
        {
            return new CatalogItemDTO(item.Id, item.CatalogGroupId, item.CatalogId, item.DisplayName, item.Number, item.Name, item.Description, item.IsActive,
                catalogGroup.Name, [.. item.Prices], [.. item.CustomerPrices], item.Unit, item.GetCurrentPrice());
        }
    }
}