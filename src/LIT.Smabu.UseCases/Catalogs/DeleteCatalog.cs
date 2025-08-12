using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Catalogs
{
    public static class DeleteCatalog
    {
        public record DeleteCatalogCommand(CatalogId CatalogId) : ICommand;

        public class DeleteCatalogHandler(IUnitOfWork uow) : ICommandHandler<DeleteCatalogCommand>
        {
            public async Task<Result> Handle(DeleteCatalogCommand request, CancellationToken cancellationToken)
            {
                Catalog catalog = await uow.Repository.GetByAsync(request.CatalogId);
                Result result = catalog.Delete();
                await uow.Repository.DeleteAsync(catalog);
                return result;
            }
        }
    }
}
