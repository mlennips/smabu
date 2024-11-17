using System;
using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.FinancialAggregate.Specifications;
using LIT.Smabu.Domain.PaymentAggregate;

namespace LIT.Smabu.Domain.FinancialAggregate.Services
{
    public class FinancialRelationsService(IAggregateStore store)
    {
        public async Task<Result> CheckCanChangeRelatedItemAsync(PaymentId paymentId)
        {
            AnnualFinancialStatement? detectedFinancialStatement = await DetectFinancialStatementAsync(paymentId);
            return detectedFinancialStatement == null
                ? Result.Success()
                : detectedFinancialStatement.Status == FinancialStatementStatus.Completed
                    ? FinancialErrors.FinancialStatementAlreadyCompleted
                    : Result.Success();
        }

        public async Task<Result> CheckCanUseDateAsync(DateTime handleDate)
        {
            return await CheckFiscalYearExistsAsync(handleDate.Year);
        }

        private async Task<Result> CheckFiscalYearExistsAsync(int fiscalYear)
        {
            AnnualFinancialStatement[] detectedFinancialStatements = await store.ApplySpecificationTask(new DectecFinancialStatementByFiscalYearSpec(fiscalYear));
            AnnualFinancialStatement? detectedFinancialStatement = detectedFinancialStatements.SingleOrDefault();
            return detectedFinancialStatement == null
                ? Result.Success()
                : detectedFinancialStatement.Status == FinancialStatementStatus.Completed
                    ? FinancialErrors.FinancialStatementAlreadyCompleted
                    : Result.Success();
        }

        private async Task<AnnualFinancialStatement?> DetectFinancialStatementAsync(PaymentId paymentId)
        {
            AnnualFinancialStatement[] detectedFinancialStatements = await store.ApplySpecificationTask(new DectecFinancialStatementByPaymentIdSpec(paymentId));
            AnnualFinancialStatement? detectedFinancialStatement = detectedFinancialStatements.SingleOrDefault();
            return detectedFinancialStatement;
        }
    }
}
