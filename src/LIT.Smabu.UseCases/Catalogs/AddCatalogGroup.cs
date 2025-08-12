using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Catalogs
{
    public static class AddCatalogGroup
    {
        public record AddCatalogGroupCommand(CatalogId CatalogId, CatalogGroupId CatalogGroupId, string Name, string Description) : ICommand;

        public class AddCatalogGroupHandler(IUnitOfWork uow) : ICommandHandler<AddCatalogGroupCommand>
        {
            public async Task<Result> Handle(AddCatalogGroupCommand request, CancellationToken cancellationToken)
            {
                Catalog catalog = await uow.Repository.GetByAsync(request.CatalogId);
                Result<CatalogGroup> groupResult = catalog.AddGroup(request.CatalogGroupId, request.Name, request.Description);
                await uow.Repository.UpdateAsync(catalog);
                return groupResult;
            }
        }
    }
}
