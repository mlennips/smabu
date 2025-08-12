using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Catalogs
{
    public static class UpdateCatalog
    {
        public record UpdateCatalogCommand(CatalogId CatalogId, string Name) : ICommand;

        public class UpdateCatalogHandler(IUnitOfWork uow) : ICommandHandler<UpdateCatalogCommand>
        {
            public async Task<Result> Handle(UpdateCatalogCommand request, CancellationToken cancellationToken)
            {
                Catalog catalog = await uow.Repository.GetByAsync(request.CatalogId);
                Result updateResult = catalog.Update(request.Name);
                await uow.Repository.UpdateAsync(catalog);
                return updateResult;
            }
        }
    }
}
