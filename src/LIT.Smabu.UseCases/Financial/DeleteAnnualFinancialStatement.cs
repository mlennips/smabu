using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.FinancialAggregate;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Financial
{
    public static class DeleteAnnualFinancialStatement
    {
        public record DeleteAnnualFinancialStatementCommand(AnnualFinancialStatementId AnnualFinancialStatementId) : ICommand;

        public class DeleteAnnualFinancialStatementHandler(IUnitOfWork uow) : ICommandHandler<DeleteAnnualFinancialStatementCommand>
        {
            public async Task<Result> Handle(DeleteAnnualFinancialStatementCommand request, CancellationToken cancellationToken)
            {
                AnnualFinancialStatement financialStatement = await uow.Repository.GetByAsync(request.AnnualFinancialStatementId);
                if (financialStatement == null)
                {
                    return FinancialErrors.FinancialStatementNotFound;
                }

                await uow.Repository.DeleteAsync(financialStatement);
                return Result.Success();
            }
        }
    }
}