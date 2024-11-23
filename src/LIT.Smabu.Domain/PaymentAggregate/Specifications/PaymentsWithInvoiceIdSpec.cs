using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.PaymentAggregate.Specifications
{
    public class PaymentsWithInvoiceIdSpec(InvoiceId invoiceId) : Specification<Payment>(x => x.InvoiceId == invoiceId);
}
