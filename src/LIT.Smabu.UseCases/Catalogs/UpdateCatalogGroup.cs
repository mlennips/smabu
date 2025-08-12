using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Catalogs
{
    public static class UpdateCatalogGroup
    {
        public record UpdateCatalogGroupCommand(CatalogId CatalogId, CatalogGroupId CatalogGroupId, string Name, string Description) : ICommand;

        public class UpdateCatalogGroupHandler(IUnitOfWork uow) : ICommandHandler<UpdateCatalogGroupCommand>
        {
            public async Task<Result> Handle(UpdateCatalogGroupCommand request, CancellationToken cancellationToken)
            {
                Catalog catalog = await uow.Repository.GetByAsync(request.CatalogId);
                Result updateResult = catalog.UpdateGroup(request.CatalogGroupId, request.Name, request.Description);
                await uow.Repository.UpdateAsync(catalog);
                return updateResult;
            }
        }
    }
}
