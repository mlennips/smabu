using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Domain.PaymentAggregate.Specifications;
using LIT.Smabu.UseCases.Base;
using LIT.Smabu.UseCases.Payments;

namespace LIT.Smabu.UseCases.Invoices
{
    public static class GetInvoice
    {
        public record GetInvoiceQuery(InvoiceId InvoiceId, bool WithItems = false) : IQuery<GetInvoiceDTO>;

        public class GetInvoiceHandler(IAggregateStore store, IAggregateCache cache) : IQueryHandler<GetInvoiceQuery, GetInvoiceDTO>
        {
            public async Task<Result<GetInvoiceDTO>> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
            {
                Invoice invoice = await store.GetByAsync(request.InvoiceId);
                Customer customer = await cache.GetByAsync(invoice.CustomerId);
                IReadOnlyList<Payment> payments = await store.ApplySpecificationTask(new PaymentsWithInvoiceIdSpec([request.InvoiceId]));
                var result = GetInvoiceDTO.Create(invoice, customer, [.. payments], request.WithItems);
                return result;
            }
        }

        public record GetInvoiceDTO : InvoiceDTO
        {
            public PaymentDTO[] Payments { get; set; }

            public bool IsPaid => Payments.All(x => x.Status == PaymentStatus.Paid);

            protected GetInvoiceDTO(InvoiceDTO original, PaymentDTO[] payments) : base(original)
            {
                Payments = payments;
            }

            public static GetInvoiceDTO Create(Invoice invoice, Customer customer, Payment[] payments, bool withItems = false)
            {
                var invoiceDto = InvoiceDTO.Create(invoice, customer, withItems);
                var paymentsDto = payments != null ? payments.Select(x =>  PaymentDTO.Create(x)).ToArray() : [];
                return new GetInvoiceDTO(invoiceDto, paymentsDto);
            }
        }
    }
}