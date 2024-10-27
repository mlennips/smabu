﻿using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.InvoiceAggregate.Specifications;
using LIT.Smabu.Domain.OfferAggregate.Specifications;
using LIT.Smabu.Domain.Shared;
using LIT.Smabu.Shared;

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

            var customer = await store.GetByAsync(id);
            customer.Delete();
            await store.DeleteAsync(customer);
            return Result.Success();
        }

        private async Task<bool> CheckHasOffers(CustomerId id)
        {
            var offers = await store.ApplySpecification(new OffersByCustomerIdSpec(id));
            return offers.Any();
        }

        private async Task<bool> CheckHasInvoices(CustomerId id)
        {
            var invoices = await store.ApplySpecification(new InvoicesByCustomerIdSpec(id));
            return invoices.Any();
        }
    }
}
