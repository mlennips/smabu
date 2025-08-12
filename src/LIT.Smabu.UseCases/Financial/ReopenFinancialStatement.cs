using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.FinancialAggregate;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Financial
{
    public static class ReopenFinancialStatement
    {
        public record ReopenFinancialStatementCommand(AnnualFinancialStatementId AnnualFinancialStatementId) : ICommand;

        public class ReopenFinancialStatementHandler(IUnitOfWork uow) : ICommandHandler<ReopenFinancialStatementCommand>
        {
            public async Task<Result> Handle(ReopenFinancialStatementCommand request, CancellationToken cancellationToken)
            {
                AnnualFinancialStatement financialStatement = await uow.Repository.GetByAsync(request.AnnualFinancialStatementId);
                if (financialStatement == null)
                {
                    return FinancialErrors.FinancialStatementNotFound;
                }
                Result reopenResult = financialStatement.Reopen();
                if (reopenResult.IsSuccess)
                {
                    await uow.Repository.UpdateAsync(financialStatement);
                }
                return reopenResult;
            }
        }
    }
}