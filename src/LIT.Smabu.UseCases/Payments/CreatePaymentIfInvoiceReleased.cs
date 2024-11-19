using System;
using LIT.Smabu.Core;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.InvoiceAggregate.Events;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Domain.PaymentAggregate.Specifications;
using MediatR;
using static LIT.Smabu.UseCases.Payments.CreatePayment;

namespace LIT.Smabu.UseCases.Payments
{
    public static class CreatePaymentIfInvoiceReleased
    {
        public class CreatePaymentIfInvoiceReleasedHandler(IAggregateStore store, ISender sender)
            : IRequestHandler<InvoiceReleasedEvent>
        {

            public async Task Handle(InvoiceReleasedEvent request, CancellationToken cancellationToken)
            {
                var alreadyExists = await CheckPaymentForInvoiceAlreadyExistsAsync(request.InvoiceId);
                if (alreadyExists)
                {
                    return;
                }
                Invoice invoice = await store.GetByAsync(request.InvoiceId);
                Customer customer = await store.GetByAsync(invoice.CustomerId);
                var command = CreatePaymentCommand.Create(invoice, customer);
                await sender.Send(command, cancellationToken);
            }

            private async Task<bool> CheckPaymentForInvoiceAlreadyExistsAsync(InvoiceId invoiceId)
            {
                IReadOnlyList<Payment> detectedPayments = await store.ApplySpecificationTask(new PaymentsWithInvoiceIdSpec(invoiceId));
                return detectedPayments.Any();
            }
        }
    }
}
