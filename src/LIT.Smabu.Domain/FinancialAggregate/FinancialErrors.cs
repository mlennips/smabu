using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.FinancialAggregate
{
    public static class FinancialErrors
    {
        public static ErrorDetail FinancialStatementAlreadyCompleted => new("FinancialStatement.AlreadyCompledted", "The financial statement is already completed.");
        public static ErrorDetail FinancialStatementAlreadyOpen => new("FinancialStatement.AlreadyOpen", "The financial statement is already open.");
        public static ErrorDetail InvalidCategoryInTransaction => new("FinancialStatement.InvalidCategoryInTransaction", "The category of transaction is invalid.");
        public static ErrorDetail FinancialStatementNotFound => new("FinancialStatement.NotFound", "The financial statement is not found.");
    }
}