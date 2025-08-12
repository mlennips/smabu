using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.FinancialAggregate
{
    public static class FinancialErrors
    {
        internal static readonly string InvalidPeriodText = "Invalid period.";
        internal static readonly string InvalidAmountText = "Amount must be greater than 0.";
        internal static readonly string InvalidDescriptionText = "Description is empty.";
        internal static readonly string InvalidCategoriesText = "Invalid categories.";

        public static ErrorDetail FinancialStatementAlreadyCompleted => new("FinancialStatement.AlreadyCompleted", "The financial statement is already completed.");
        public static ErrorDetail FinancialStatementAlreadyOpen => new("FinancialStatement.AlreadyOpen", "The financial statement is already open.");
        public static ErrorDetail InvalidTransaction => new("FinancialStatement.InvalidTransaction", "Transaction is invalid.");
        public static ErrorDetail ManipulatedImportedValues => new("FinancialStatement.ManipulatedImportedValues", "The imported values are manipulated.");
        public static ErrorDetail FinancialStatementNotFound => new("FinancialStatement.NotFound", "The financial statement is not found.");
        public static ErrorDetail FiscalYearAlreadyExists => new("FinancialStatement.FiscalYearAlreadyExists", "The fiscal year already exists.");
        public static ErrorDetail ImportTransactionsFailed => new("FinancialStatement.ImportTransactionsFailed", "The import of transactions failed.");
        public static ErrorDetail MandatoryRelationsNotMet => new("FinancialStatement.MandatoryRelationsNotMet", "The mandatory relations are not met.");
    }
}