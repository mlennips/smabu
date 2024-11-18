using System;
using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.PaymentAggregate.Specifications
{
    public class DetectPaymentsForFiscalYearSpec(int fiscalYear)
        : Specification<Payment>(x => x.AccountingDate.HasValue
            && x.AccountingDate.Value >= new DateTime(fiscalYear, 1, 1)
            && x.AccountingDate.Value <= new DateTime(fiscalYear, 12, 31));
}
