﻿using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.Shared;

namespace LIT.Smabu.Domain.CatalogAggregate
{
    public class Catalog(CatalogId id, string name, List<CatalogGroup> groups) : AggregateRoot<CatalogId>
    {
        private readonly List<CatalogGroup> _groups = groups;

        public override CatalogId Id { get; } = id;
        public string Name { get; private set; } = name;
        public IReadOnlyList<CatalogGroup> Groups  => _groups;

        public static Catalog Create(CatalogId id, string name)
        {
            var catalog = new Catalog(id, name, []);
            return catalog;
        }

        public Result Update(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return CatalogErrors.NameEmpty;
            }
            Name = name;
            return Result.Success();
        }

        public Result<CatalogGroup> GetGroup(CatalogGroupId id)
        {
            var group = Groups.FirstOrDefault(g => g.Id == id);
            return group != null
                ? group
                : CatalogErrors.GroupNotFound;
        }

        public Result<CatalogGroup> AddGroup(CatalogGroupId id, string name, string description)
        {
            if (string.IsNullOrEmpty(name))
            {
                return CatalogErrors.NameEmpty;
            }
            if (Groups.Any(g => g.Name == name))
            {
                return CatalogErrors.NameAlreadyExists;
            }
            var group = CatalogGroup.Create(id, Id, name, description);
            _groups.Add(group);
            return group;
        }

        public Result RemoveGroup(CatalogGroupId id)
        {
            var group = Groups.FirstOrDefault(g => g.Id == id);
            if (group == null)
            {
                return Result.Failure(CatalogErrors.GroupNotFound);
            }
            if (group.Items.Count > 0)
            {
                return Result.Failure(CatalogErrors.GroupNotEmpty);
            }
            _groups.Remove(group);
            return Result.Success();
        }

        public Result UpdateGroup(CatalogGroupId id, string name, string description)
        {
            var group = Groups.FirstOrDefault(g => g.Id == id);
            if (group == null)
            {
                return Result.Failure(CatalogErrors.GroupNotFound);
            }
            group?.Update(name, description);
            return Result.Success();
        }

        public Result<CatalogItem> GetItem(CatalogItemId id)
        {
            var item = Groups.SelectMany(g => g.Items).FirstOrDefault(i => i.Id == id);
            
            return item == null
             ? CatalogErrors.ItemNotFound
             : item;
        }

        public Result UpdateItem(CatalogItemId id, string name, string description, bool isActive, Unit unit,
            CatalogItemPrice[] prices)
        {
            var itemResult = GetItem(id);
            if (itemResult.IsFailure)
            {
                return Result.Failure(itemResult.Error);
            }
            else
            {
                var item = itemResult.Value!;
                var updateResult = item.Update(name, description, isActive, unit);
                var pricesResult = item.UpdatePrices([.. prices]);

                return updateResult.IsSuccess && pricesResult.IsSuccess
                    ? Result.Success()
                    : Result.Failure([updateResult.Error, pricesResult.Error]);
            }
        }

        public Result<CatalogItem> AddItem(CatalogItemId id, CatalogGroupId catalogGroupId, string name, string description)
        {
            if (string.IsNullOrEmpty(name))
            {
                return CatalogErrors.NameEmpty;
            }
            if (Groups.SelectMany(g => g.Items).Any(i => i.Name == name))
            {
                return CatalogErrors.NameAlreadyExists;
            }
            var group = Groups.Single(x => x.Id == catalogGroupId);
            var lastNumber = Groups.SelectMany(g => g.Items)
                .Select(i => i.Number)
                .OrderByDescending(i => i)
                .FirstOrDefault();
            var number = lastNumber != null ? CatalogItemNumber.CreateNext(lastNumber) : CatalogItemNumber.CreateFirst();
            var result = group.AddItem(id, number, name, description, Unit.None);
            return result.IsSuccess
                ? result.Value!
                : result.Error;
        }

        public Result RemoveItem(CatalogItemId id)
        {
            var group = Groups.Single(g => g.Items.Any(i => i.Id == id));
            var result = group.RemoveItem(id);
            return result;
        }
    }
}
