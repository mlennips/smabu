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
        public record CreateAnnualFinancialStatementCommand(AnnualFinancialStatementId Id, int FiscalYear, DateOnly StartDate, DateOnly EndDate,
            List<Transaction> Incomes, List<Transaction> Expenditures, FinancialStatementStatus Status) : ICommand<AnnualFinancialStatementId>;

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

                var financialStatement = new AnnualFinancialStatement(request.Id, request.FiscalYear, request.StartDate, request.EndDate, request.Incomes, request.Expenditures, request.Status);
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