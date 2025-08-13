using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.OrderAggregate.Specifications;
using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.InvoiceAggregate.Services
{
    public class DeleteInvoiceService(IUnitOfWork uow)
    {
        public async Task<Result> DeleteAsync(InvoiceId id)
        {
            var hasRelations = await CheckIsInOrdersAsync(id);
            if (hasRelations)
            {
                return CommonErrors.HasReferences;
            }

            Invoice invoice = await uow.Repository.GetByAsync(id);
            invoice.Delete();
            await uow.Repository.DeleteAsync(invoice);
            return Result.Success();
        }

        private async Task<bool> CheckIsInOrdersAsync(InvoiceId id)
        {
            IReadOnlyList<OrderAggregate.Order> orders = await uow.Repository.ApplySpecificationTask(new DetectOrderForReferenceIdSpec(id));
            return orders.Any();
        }
    }
}
