using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.FinancialAggregate;
using LIT.Smabu.Domain.FinancialAggregate.Services;
using LIT.Smabu.Domain.FinancialAggregate.Specifications;
using LIT.Smabu.UseCases.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LIT.Smabu.UseCases.Financial
{
    public static class CreateAnnualFinancialStatement
    {
        public record CreateAnnualFinancialStatementCommand(AnnualFinancialStatementId AnnualFinancialStatementId, int FiscalYear) : ICommand<AnnualFinancialStatementId>;

        public class CreateAnnualFinancialStatementHandler(IAggregateStore store)
            : ICommandHandler<CreateAnnualFinancialStatementCommand, AnnualFinancialStatementId>
        {
            public async Task<Result<AnnualFinancialStatementId>> Handle(CreateAnnualFinancialStatementCommand request, CancellationToken cancellationToken)
            {
                Result checkFiscalYearResult = await CheckFiscalYearAlreadyExistsAsync(request);
                if (checkFiscalYearResult.IsFailure)
                {
                    return checkFiscalYearResult.Error;
                }

                var financialStatement = AnnualFinancialStatement.Create(request.AnnualFinancialStatementId, request.FiscalYear);
                await store.CreateAsync(financialStatement);
                return Result.Success(financialStatement.Id);
            }

            private async Task<Result> CheckFiscalYearAlreadyExistsAsync(CreateAnnualFinancialStatementCommand request)
            {
                AnnualFinancialStatement[] detectedFinancialStatements = await store.ApplySpecificationTask(new DectecFinancialStatementByFiscalYearSpec(request.FiscalYear));
                return detectedFinancialStatements.SingleOrDefault() != null
                    ? (Result)FinancialErrors.FiscalYearAlreadyExists
                    : Result.Success();
            }
        }
    }
}