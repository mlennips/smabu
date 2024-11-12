using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Shared;

namespace LIT.Smabu.Domain.PaymentAggregate.Specifications
{
    public class DetectPaymentsWithInvoiceIdSpec(InvoiceId invoiceId) : Specification<Payment>(x => x.InvoiceId == invoiceId);
}
