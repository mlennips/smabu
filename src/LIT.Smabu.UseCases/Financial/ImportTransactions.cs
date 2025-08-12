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

        public class ImportTransactionsHandler(IUnitOfWork uow) : ICommandHandler<ImportAnnualFinancialStatementTransactionsCommand>
        {
            public async Task<Result> Handle(ImportAnnualFinancialStatementTransactionsCommand request, CancellationToken cancellationToken)
            {
                AnnualFinancialStatement annualFinancialStatement
                    = await uow.Repository.GetByAsync(request.AnnualFinancialStatementId);
                Payment[] detectedPayments
                    = await uow.Repository.ApplySpecificationTask(new PaymentsForFiscalYearSpec(annualFinancialStatement.FiscalYear));

                Result result = annualFinancialStatement.ImportIncomes(detectedPayments);
                if (result.IsFailure)
                {
                    return FinancialErrors.ImportTransactionsFailed;
                }

                await uow.Repository.UpdateAsync(annualFinancialStatement);
                return Result.Success();
            }
        }

    }
}
