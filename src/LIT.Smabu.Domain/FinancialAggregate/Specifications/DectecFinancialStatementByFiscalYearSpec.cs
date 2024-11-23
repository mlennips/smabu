using System;
using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.FinancialAggregate.Specifications
{
    public class DectecFinancialStatementByFiscalYearSpec : Specification<AnnualFinancialStatement>
    {
        public DectecFinancialStatementByFiscalYearSpec(int FiscalYear)
            : base(x => x.FiscalYear == FiscalYear)
        {
            Take = 1;
        }
    }
}