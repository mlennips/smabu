using System;
using System.Collections.Generic;
using System.Linq;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;

namespace LIT.Smabu.Domain.FinancialAggregate
{
    public class AnnualFinancialStatement(AnnualFinancialStatementId id, int fiscalYear, DateOnly startDate, DateOnly endDate,
        List<Transaction> incomes, List<Transaction> expenditures, FinancialStatementStatus status)
        : AggregateRoot<AnnualFinancialStatementId>
    {
        public override AnnualFinancialStatementId Id { get; } = id;
        public int FiscalYear { get; private set; } = fiscalYear;
        public DateOnly StartDate { get; } = startDate;
        public DateOnly EndDate { get; } = endDate;
        public Currency Currency { get; private set; } = Currency.EUR;
        public List<Transaction> Incomes { get; private set; } = incomes;
        public List<Transaction> Expenditures { get; private set; } = expenditures;
        public FinancialStatementStatus Status { get; private set; } = status;

        public decimal TotalIncome => Incomes.Sum(t => t.Amount);
        public decimal TotalExpenditure => Expenditures.Sum(t => t.Amount);
        public decimal NetIncome => TotalIncome - TotalExpenditure;

        public Result UpdateIncomes(Transaction[] incomes)
        {
            if (incomes.Any(income => !FinancialCategory.CheckIsIncome(income.Category)))
            {
                return FinancialErrors.InvalidCategoryInTransaction;
            }
            Incomes.Clear();
            Incomes.AddRange(incomes);
            return Result.Success();
        }

        public Result UpdateExpenditures(Transaction[] expenditures)
        {
            if (expenditures.Any(expenditure => !FinancialCategory.CheckIsIncome(expenditure.Category)))
            {
                return FinancialErrors.InvalidCategoryInTransaction;
            }
            Expenditures.Clear();
            Expenditures.AddRange(expenditures);
            return Result.Success();
        }

        public Result Complete()
        {
            if (Status == FinancialStatementStatus.Completed)
            {
                return FinancialErrors.FinancialStatementAlreadyCompleted;
            }

            Status = FinancialStatementStatus.Completed;
            return Result.Success();
        }

        public Result Reopen()
        {
            if (Status == FinancialStatementStatus.Open)
            {
                return FinancialErrors.FinancialStatementAlreadyOpen;
            }

            Status = FinancialStatementStatus.Open;
            return Result.Success();
        }
    }
}