using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Core;
using System;
using System.Linq;

namespace LIT.Smabu.Domain.PaymentAggregate.Specifications
{
    public class PaymentsWithInvoiceIdSpec(InvoiceId[] invoiceIds) : Specification<Payment>(x => invoiceIds.Contains(x.InvoiceId));
}
