using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.InvoiceAggregate.Specifications;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Domain.OfferAggregate.Specifications;
using System.Threading;

namespace LIT.Smabu.Domain.CustomerAggregate.Services
{
    public class DeleteCustomerService(IUnitOfWork uow)
    {
        public async Task<Result> DeleteAsync(CustomerId id)
        {
            var hasRelations = await CheckHasOffers(id) || await CheckHasInvoices(id);
            if (hasRelations)
            {
                return CommonErrors.HasReferences;
            }

            Customer customer = await uow.Repository.GetByAsync(id);
            customer.Delete();
            await uow.Repository.DeleteAsync(customer);
            return Result.Success();
        }

        private async Task<bool> CheckHasOffers(CustomerId id)
        {
            IReadOnlyList<Offer> offers = await uow.Repository.ApplySpecificationTask(new OffersByCustomerIdSpec(id));
            return offers.Any();
        }

        private async Task<bool> CheckHasInvoices(CustomerId id)
        {
            IReadOnlyList<Invoice> invoices = await uow.Repository.ApplySpecificationTask(new InvoicesByCustomerIdSpec(id));
            return invoices.Any();
        }
    }
}
