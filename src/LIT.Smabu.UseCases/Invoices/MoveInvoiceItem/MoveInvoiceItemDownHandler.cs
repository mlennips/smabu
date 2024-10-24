﻿using LIT.Smabu.Domain.Shared;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Invoices.MoveInvoiceItem
{
    public class MoveInvoiceItemDownHandler(IAggregateStore aggregateStore) : ICommandHandler<MoveInvoiceItemDownCommand>
    {
        public async Task<Result> Handle(MoveInvoiceItemDownCommand request, CancellationToken cancellationToken)
        {
            var invoice = await aggregateStore.GetByAsync(request.InvoiceId);
            var result = invoice.MoveItemDown(request.Id);
            if (result.IsFailure)
            {
                return result.Error;
            }

            await aggregateStore.UpdateAsync(invoice);
            return Result.Success();
        }
    }
}
