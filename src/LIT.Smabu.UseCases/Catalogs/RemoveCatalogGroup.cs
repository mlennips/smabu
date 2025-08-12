using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Catalogs
{
    public static class RemoveCatalogGroup
    {
        public record RemoveCatalogGroupCommand(CatalogGroupId CatalogGroupId, CatalogId CatalogId) : ICommand;
        public class RemoveCatalogGroupHandler(IUnitOfWork uow) : ICommandHandler<RemoveCatalogGroupCommand>
        {

            public async Task<Result> Handle(RemoveCatalogGroupCommand request, CancellationToken cancellationToken)
            {
                Catalog catalog = await uow.Repository.GetByAsync(request.CatalogId);
                Result result = catalog.RemoveGroup(request.CatalogGroupId);
                await uow.Repository.UpdateAsync(catalog);
                return result;
            }
        }
    }
}
