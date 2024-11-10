﻿using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.Services;
using LIT.Smabu.Domain.Shared;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Invoices.Release
{
    public class ReleaseInvoiceHandler(IAggregateStore store, BusinessNumberService businessNumberService) : ICommandHandler<ReleaseInvoiceCommand>
    {
        public async Task<Result> Handle(ReleaseInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = await store.GetByAsync(request.Id);
            InvoiceNumber number = await businessNumberService.CreateInvoiceNumberAsync(invoice.Number, request.Number, invoice.FiscalYear);
            var result = invoice.Release(number, request.ReleasedAt);
            if (result.IsFailure)
            {
                return result.Error;
            }

            await store.UpdateAsync(invoice);
            return Result.Success();
        }
    }
}
