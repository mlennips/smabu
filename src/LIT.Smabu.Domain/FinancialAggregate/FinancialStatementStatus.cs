using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.FinancialAggregate
{
    public record class FinancialStatementStatus : SimpleValueObject<string>
    {
        public FinancialStatementStatus(string value) : base(value)
        {

        }

        public static FinancialStatementStatus Open => new("Open");
        public static FinancialStatementStatus Completed => new("Completed");
    }
}
