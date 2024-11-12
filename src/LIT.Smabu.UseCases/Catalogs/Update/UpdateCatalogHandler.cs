using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Catalogs.Update
{
    public class UpdateCatalogHandler(IAggregateStore store) : ICommandHandler<UpdateCatalogCommand>
    {
        public async Task<Result> Handle(UpdateCatalogCommand request, CancellationToken cancellationToken)
        {
            var catalog = await store.GetByAsync(request.CatalogId);
            var updateResult = catalog.Update(request.Name);
            await store.UpdateAsync(catalog);
            return updateResult;
        }
    }
}
