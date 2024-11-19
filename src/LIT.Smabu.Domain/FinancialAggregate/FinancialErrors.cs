using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.FinancialAggregate
{
    public static class FinancialErrors
    {
        public static ErrorDetail FinancialStatementAlreadyCompleted => new("FinancialStatement.AlreadyCompledted", "The financial statement is already completed.");
        public static ErrorDetail FinancialStatementAlreadyOpen => new("FinancialStatement.AlreadyOpen", "The financial statement is already open.");
        public static ErrorDetail InvalidTransaction => new("FinancialStatement.InvalidTransaction", "Transaction is invalid.");
        public static ErrorDetail FinancialStatementNotFound => new("FinancialStatement.NotFound", "The financial statement is not found.");
        public static ErrorDetail FiscalYearAlreadyExists => new("FinancialStatement.FiscalYearAlreadyExists", "The fiscal year already exists.");
        public static ErrorDetail ImportTransactionsFailed => new("FinancialStatement.ImportTransactionsFailed", "The import of transactions failed.");
    }
}