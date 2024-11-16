using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.FinancialAggregate;
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

                Result result = financialStatement.Complete();
                await store.UpdateAsync(financialStatement);
                return result;
            }
        }
    }
}