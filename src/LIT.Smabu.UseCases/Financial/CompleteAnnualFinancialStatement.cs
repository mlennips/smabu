using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.FinancialAggregate;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Domain.PaymentAggregate.Specifications;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Financial
{
    public static class CompleteAnnualFinancialStatement
    {
        public record CompleteAnnualFinancialStatementCommand(AnnualFinancialStatementId AnnualFinancialStatementId) : ICommand;

        public class CompleteAnnualFinancialStatementHandler(IAggregateStore store) : ICommandHandler<CompleteAnnualFinancialStatementCommand>
        {
            public async Task<Result> Handle(CompleteAnnualFinancialStatementCommand request, CancellationToken cancellationToken)
            {
                AnnualFinancialStatement financialStatement = await store.GetByAsync(request.AnnualFinancialStatementId);
                if (financialStatement == null)
                {
                    return FinancialErrors.FinancialStatementNotFound;
                }

                Result checkMandatoryRelations = await CheckMandatoryRelationsAsync(financialStatement);
                if (checkMandatoryRelations.IsFailure)
                {
                    return checkMandatoryRelations;
                }

                Result result = financialStatement.Complete();
                await store.UpdateAsync(financialStatement);
                return result;
            }

            private async Task<Result> CheckMandatoryRelationsAsync(AnnualFinancialStatement financialStatement)
            {
                Payment[] paymentsForFiscalYear = await store.ApplySpecificationTask(new PaymentsForFiscalYearSpec(financialStatement.FiscalYear));
                var importedPayments = financialStatement.Incomes.Where(x => x.PaymentId != null).ToList();
                var hasSameAmount = paymentsForFiscalYear.Sum(x => x.AmountPaid) == importedPayments.Sum(x => x.Amount);
                var hasSameCount = paymentsForFiscalYear.Length == importedPayments.Count;
                return !hasSameAmount || !hasSameCount
                    ? FinancialErrors.MandatoryRelationsNotMet
                    : Result.Success();
            }
        }
    }
}