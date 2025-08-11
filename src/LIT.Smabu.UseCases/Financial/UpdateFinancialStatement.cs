using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.FinancialAggregate;
using LIT.Smabu.UseCases.Base;
using MediatR;

namespace LIT.Smabu.UseCases.Financial
{
    public static class UpdateAnnualFinancialStatement
    {
        public record UpdateAnnualFinancialStatementCommand(AnnualFinancialStatementId AnnualFinancialStatementId,
            List<FinancialTransaction> Incomes, List<FinancialTransaction> Expenditures) : ICommand;

        public class UpdateAnnualFinancialStatementHandler(IAggregateRepository repository) : ICommandHandler<UpdateAnnualFinancialStatementCommand>
        {

            public async Task<Result> Handle(UpdateAnnualFinancialStatementCommand request, CancellationToken cancellationToken)
            {
                AnnualFinancialStatement financialStatement = await repository.GetByAsync(request.AnnualFinancialStatementId);
                if (financialStatement == null)
                {
                    return FinancialErrors.FinancialStatementNotFound;
                }

                Result incomeResult = financialStatement.UpdateIncomes([.. request.Incomes]);
                if (incomeResult.IsFailure)
                {
                    return incomeResult;
                }

                Result expenditureResult = financialStatement.UpdateExpenditures([.. request.Expenditures]);
                if (expenditureResult.IsFailure)
                {
                    return expenditureResult;
                }

                await repository.UpdateAsync(financialStatement);

                return Result.Success();
            }
        }
    }
}