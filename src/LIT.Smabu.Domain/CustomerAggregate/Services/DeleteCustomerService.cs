using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.InvoiceAggregate.Specifications;
using LIT.Smabu.Domain.OfferAggregate.Specifications;
using LIT.Smabu.Core;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.OfferAggregate;

namespace LIT.Smabu.Domain.CustomerAggregate.Services
{
    public class DeleteCustomerService(IAggregateStore store)
    {
        public async Task<Result> DeleteAsync(CustomerId id)
        {
            var hasRelations = await CheckHasOffers(id) || await CheckHasInvoices(id);
            if (hasRelations)
            {
                return CommonErrors.HasReferences;
            }

            Customer customer = await store.GetByAsync(id);
            customer.Delete();
            await store.DeleteAsync(customer);
            return Result.Success();
        }

        private async Task<bool> CheckHasOffers(CustomerId id)
        {
            IReadOnlyList<Offer> offers = await store.ApplySpecificationTask(new OffersByCustomerIdSpec(id));
            return offers.Any();
        }

        private async Task<bool> CheckHasInvoices(CustomerId id)
        {
            IReadOnlyList<Invoice> invoices = await store.ApplySpecificationTask(new InvoicesByCustomerIdSpec(id));
            return invoices.Any();
        }
    }
}
