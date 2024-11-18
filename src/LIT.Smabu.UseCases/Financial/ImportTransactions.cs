using System;
using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.FinancialAggregate;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Domain.PaymentAggregate.Specifications;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Financial
{
    public static class ImportAnnualFinancialStatementTransactions
    {
        public record ImportAnnualFinancialStatementTransactionsCommand(AnnualFinancialStatementId AnnualFinancialStatementId) : ICommand;

        public class ImportTransactionsHandler(IAggregateStore store) : ICommandHandler<ImportAnnualFinancialStatementTransactionsCommand>
        {
            public async Task<Result> Handle(ImportAnnualFinancialStatementTransactionsCommand request, CancellationToken cancellationToken)
            {
                AnnualFinancialStatement annualFinancialStatement
                    = await store.GetByAsync(request.AnnualFinancialStatementId);
                Payment[] detectedPayments
                    = await store.ApplySpecificationTask(new DetectPaymentsForFiscalYearSpec(annualFinancialStatement.FiscalYear));

                Result result = annualFinancialStatement.ImportIncomes(detectedPayments);
                if (result.IsFailure)
                {
                    return FinancialErrors.ImportTransactionsFailed;
                }

                await store.UpdateAsync(annualFinancialStatement);
                return Result.Success();
            }
        }

    }
}
