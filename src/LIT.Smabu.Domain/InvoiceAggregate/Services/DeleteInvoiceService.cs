﻿using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.OrderAggregate.Specifications;
using LIT.Smabu.Domain.Shared;
using LIT.Smabu.Shared;

namespace LIT.Smabu.Domain.InvoiceAggregate.Services
{
    public class DeleteInvoiceService(IAggregateStore store)
    {
        public async Task<Result> DeleteAsync(InvoiceId id)
        {
            var hasRelations = await CheckIsInOrdersAsync(id);
            if (hasRelations)
            {
                return CommonErrors.HasReferences;
            }

            var invoice = await store.GetByAsync(id);
            invoice.Delete();
            await store.DeleteAsync(invoice);
            return Result.Success();
        }

        private async Task<bool> CheckIsInOrdersAsync(InvoiceId id)
        {
            var orders = await store.ApplySpecification(new DetectOrderForReferenceIdSpec(id));
            return orders.Any();
        }
    }
}
