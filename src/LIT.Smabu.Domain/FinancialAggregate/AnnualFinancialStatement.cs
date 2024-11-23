using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.PaymentAggregate;

namespace LIT.Smabu.Domain.FinancialAggregate
{
    public class AnnualFinancialStatement(AnnualFinancialStatementId id, int fiscalYear, DatePeriod period,
        List<FinancialTransaction> incomes, List<FinancialTransaction> expenditures, FinancialStatementStatus status)
        : AggregateRoot<AnnualFinancialStatementId>
    {
        private readonly List<FinancialTransaction> _incomes = incomes;
        private readonly List<FinancialTransaction> _expenditures = expenditures;

        public override AnnualFinancialStatementId Id { get; } = id;
        public int FiscalYear { get; private set; } = fiscalYear;
        public DatePeriod Period { get; } = period;
        public Currency Currency { get; private set; } = Currency.EUR;
        public IReadOnlyList<FinancialTransaction> Incomes => _incomes;
        public IReadOnlyList<FinancialTransaction> Expenditures => _expenditures;
        public FinancialStatementStatus Status { get; private set; } = status;

        public decimal TotalIncome => Incomes.Sum(t => t.Amount);
        public decimal TotalExpenditure => Expenditures.Sum(t => t.Amount);
        public decimal NetIncome => TotalIncome - TotalExpenditure;

        public static AnnualFinancialStatement Create(AnnualFinancialStatementId id, int fiscalYear)
        {
            var period = new DatePeriod(new DateOnly(fiscalYear, 1, 1), new DateOnly(fiscalYear, 12, 31));
            return new AnnualFinancialStatement(id, fiscalYear, period, [], [], FinancialStatementStatus.Open);
        }

        public Result ImportIncomes(Payment[] payments)
        {
            Result checkCanEdit = CheckCanEdit();
            if (checkCanEdit.IsFailure)
            {
                return checkCanEdit;
            }

            var incomes = _incomes.ToList();
            incomes.RemoveAll(x => x.PaymentId != null);
            incomes.AddRange(payments.Select(FinancialTransaction.CreateForPayment));
            return UpdateIncomes([.. incomes]);
        }

        public Result UpdateIncomes(FinancialTransaction[] incomes)
        {
            Result checkCanEdit = CheckCanEdit();
            if (checkCanEdit.IsFailure)
            {
                return checkCanEdit;
            }

            if (incomes.Any(income => !FinancialCategory.CheckIsIncome(income.Category))
                || ValidateTransaction(incomes).IsFailure)
            {
                return FinancialErrors.InvalidTransaction;
            }
            var checkImportetValuesManipulated =
                _incomes.Any(existingIncome => incomes.Any(newIncome =>
                    existingIncome.PaymentId != null && existingIncome.PaymentId == newIncome.PaymentId && existingIncome.Amount != newIncome.Amount));
            if (checkImportetValuesManipulated)
            {
                return FinancialErrors.ManipulatedImportedValues;
            }

            _incomes.Clear();
            _incomes.AddRange(incomes.OrderBy(x => x.Date));
            return Result.Success();
        }

        public Result UpdateExpenditures(FinancialTransaction[] expenditures)
        {
            Result checkCanEdit = CheckCanEdit();
            if (checkCanEdit.IsFailure)
            {
                return checkCanEdit;
            }

            if (expenditures.Any(expenditure => !FinancialCategory.CheckIsExpenditure(expenditure.Category))
                || ValidateTransaction(expenditures).IsFailure)
            {
                return FinancialErrors.InvalidTransaction;
            }
            var checkImportetValuesManipulated =
                _expenditures.Any(existingExpenditures => expenditures.Any(newExpenditures =>
                    existingExpenditures.PaymentId != null && existingExpenditures.PaymentId == newExpenditures.PaymentId && existingExpenditures.Amount != newExpenditures.Amount));
            if (checkImportetValuesManipulated)
            {
                return FinancialErrors.ManipulatedImportedValues;
            }

            _expenditures.Clear();
            _expenditures.AddRange(expenditures);
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

        private Result CheckCanEdit()
        {
            return Status == FinancialStatementStatus.Completed
                ? FinancialErrors.FinancialStatementAlreadyCompleted
                : Result.Success();
        }

        private Result ValidateTransaction(FinancialTransaction[] transactions)
        {
            return transactions.Any(income => income.Date.Year != FiscalYear)
                ? (Result)FinancialErrors.InvalidTransaction
                : transactions.Any(income => string.IsNullOrWhiteSpace(income.Description))
                    ? (Result)FinancialErrors.InvalidTransaction
                    : transactions.Any(income => income.Amount <= 0)
                        ? (Result)FinancialErrors.InvalidTransaction
                        : Result.Success();
        }
    }
}