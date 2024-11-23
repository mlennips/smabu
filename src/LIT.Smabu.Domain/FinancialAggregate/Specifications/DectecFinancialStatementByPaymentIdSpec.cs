using System;
using LIT.Smabu.Core;
using LIT.Smabu.Domain.PaymentAggregate;

namespace LIT.Smabu.Domain.FinancialAggregate.Specifications
{
    public class DectecFinancialStatementByPaymentIdSpec : Specification<AnnualFinancialStatement>
    {
        public DectecFinancialStatementByPaymentIdSpec(PaymentId paymentId)
            : base(x => x.Incomes.Any(y => y.PaymentId == paymentId || x.Expenditures.Any(y => y.PaymentId == paymentId)))
        {
            Take = 1;
        }
    }
}