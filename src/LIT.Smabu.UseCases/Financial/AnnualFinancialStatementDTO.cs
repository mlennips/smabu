using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.FinancialAggregate;

namespace LIT.Smabu.UseCases.Financial
{
    public record class AnnualFinancialStatementDTO
    {
        public AnnualFinancialStatementId Id { get; init; }
        public int FiscalYear { get; init; }
        public DateOnly StartDate { get; init; }
        public DateOnly EndDate { get; init; }
        public Currency Currency { get; init; }
        public List<Transaction> Incomes { get; init; }
        public List<Transaction> Expenditures { get; init; }
        public FinancialStatementStatus Status { get; init; }
        public decimal TotalIncome { get; init; }
        public decimal TotalExpenditure { get; init; }
        public decimal NetIncome { get; init; }

        public AnnualFinancialStatementDTO(AnnualFinancialStatementId id, int fiscalYear, DateOnly startDate, DateOnly endDate, Currency currency, List<Transaction> incomes, List<Transaction> expenditures, FinancialStatementStatus status, decimal totalIncome, decimal totalExpenditure, decimal netIncome)
        {
            Id = id;
            FiscalYear = fiscalYear;
            StartDate = startDate;
            EndDate = endDate;
            Currency = currency;
            Incomes = incomes;
            Expenditures = expenditures;
            Status = status;
            TotalIncome = totalIncome;
            TotalExpenditure = totalExpenditure;
            NetIncome = netIncome;
        }

        internal static AnnualFinancialStatementDTO Create(AnnualFinancialStatement statement)
        {
            return new AnnualFinancialStatementDTO(statement.Id, statement.FiscalYear, statement.StartDate, statement.EndDate, statement.Currency, statement.Incomes, statement.Expenditures, statement.Status, statement.TotalIncome, statement.TotalExpenditure, statement.NetIncome);
        }
    }
}
