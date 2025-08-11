using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.FinancialAggregate;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Financial
{
    public static class DeleteAnnualFinancialStatement
    {
        public record DeleteAnnualFinancialStatementCommand(AnnualFinancialStatementId AnnualFinancialStatementId) : ICommand;

        public class DeleteAnnualFinancialStatementHandler(IAggregateRepository repository) : ICommandHandler<DeleteAnnualFinancialStatementCommand>
        {
            public async Task<Result> Handle(DeleteAnnualFinancialStatementCommand request, CancellationToken cancellationToken)
            {
                AnnualFinancialStatement financialStatement = await repository.GetByAsync(request.AnnualFinancialStatementId);
                if (financialStatement == null)
                {
                    return FinancialErrors.FinancialStatementNotFound;
                }

                await repository.DeleteAsync(financialStatement);
                return Result.Success();
            }
        }
    }
}