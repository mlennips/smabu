using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.FinancialAggregate;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Financial
{
    public static class ListAnnualFinancialStatements
    {
        public record ListAnnualFinancialStatementsQuery() : IQuery<AnnualFinancialStatementDTO[]>;

        public class ListAnnualFinancialStatementsHandler(IAggregateStore store)
            : IQueryHandler<ListAnnualFinancialStatementsQuery, AnnualFinancialStatementDTO[]>
        {
            public async Task<Result<AnnualFinancialStatementDTO[]>> Handle(ListAnnualFinancialStatementsQuery request, CancellationToken cancellationToken)
            {
                AnnualFinancialStatement[] annualFinancialStatements = await store.GetAllAsync<AnnualFinancialStatement>();
                return Result.Success(annualFinancialStatements.Select(AnnualFinancialStatementDTO.Create).ToArray());
            }
        }
    }
}