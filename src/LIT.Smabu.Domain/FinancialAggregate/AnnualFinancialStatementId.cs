using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.FinancialAggregate
{
    public record class AnnualFinancialStatementId(Guid Value) : EntityId<AnnualFinancialStatement>(Value)
    {

    }
}
