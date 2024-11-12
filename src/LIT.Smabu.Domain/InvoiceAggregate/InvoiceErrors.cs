using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.InvoiceAggregate
{
    public static class InvoiceErrors
    {
        public static readonly ErrorDetail AlreadyReleased = new("Invoice.AlreadyReleased", "Invoice has already been released.");
        public static readonly ErrorDetail NumberNotValid = new("Invoice.NumberNotValid", "Invoice number is invalid."); public static readonly ErrorDetail NumberMayNotBeChangedBelated = new("Invoice.NumberMayNotBeChangedBelated", "Once an invoice number has been assigned, it cannot be changed.");
        public static readonly ErrorDetail NoPositionsToRelease = new("Invoice.NoPositionsToRelease", "No items available for release.");
        public static readonly ErrorDetail NotReleasedYet = new("Invoice.NotReleasedYet", "Cannot withdraw release as it has not been released yet.");
        public static readonly ErrorDetail ItemNotFound = new("Invoice.ItemNotFound", "Item not found.");
        public static readonly ErrorDetail ItemDetailsEmpty = new("Invoice.DetailsEmpty", "Details must not be empty.");
        public static readonly ErrorDetail ItemAlreadyAtEnd = new("Invoice.ItemAlreadyAtEnd", "Already at the end of the list.");
        public static readonly ErrorDetail ItemAlreadyAtBeginning = new("Invoice.ItemAlreadyAtStart", "Already at the beginning of the list.");
    }
}
