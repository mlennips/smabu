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
        public class CreatePaymentIfInvoiceReleasedHandler(IUnitOfWork uow, ISender sender)
            : IRequestHandler<InvoiceReleasedEvent, Result>
        {
            public async Task<Result> Handle(InvoiceReleasedEvent request, CancellationToken cancellationToken)
            {
                var alreadyExists = await CheckPaymentForInvoiceAlreadyExistsAsync(request.InvoiceId);
                if (alreadyExists)
                {
                    return Result.Success();
                }
                Invoice invoice = await uow.Repository.GetByAsync(request.InvoiceId);
                Customer customer = await uow.Repository.GetByAsync(invoice.CustomerId);
                var command = CreatePaymentCommand.Create(invoice, customer);
                await sender.Send(command, cancellationToken);
                return Result.Success();
            }

            private async Task<bool> CheckPaymentForInvoiceAlreadyExistsAsync(InvoiceId invoiceId)
            {
                IReadOnlyList<Payment> detectedPayments = await uow.Repository.ApplySpecificationTask(new PaymentsWithInvoiceIdSpec([invoiceId]));
                return detectedPayments.Any();
            }
        }
    }
}
