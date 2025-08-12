using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.FinancialAggregate;
using LIT.Smabu.UseCases.Base;
using MediatR;

namespace LIT.Smabu.UseCases.Financial
{
    public static class GetAnnualFinancialStatement
    {
        public record GetAnnualFinancialStatementQuery(AnnualFinancialStatementId AnnualFinancialStatementId) : IQuery<AnnualFinancialStatementDTO>;

        public class GetAnnualFinancialStatementHandler(IAggregateRepository repository) : IQueryHandler<GetAnnualFinancialStatementQuery, AnnualFinancialStatementDTO>
        {
            public async Task<Result<AnnualFinancialStatementDTO>> Handle(GetAnnualFinancialStatementQuery request, CancellationToken cancellationToken)
            {
                AnnualFinancialStatement annualFinancialStatement = await repository.GetByAsync(request.AnnualFinancialStatementId);
                return annualFinancialStatement == null
                    ? FinancialErrors.FinancialStatementNotFound
                    : Result.Success(AnnualFinancialStatementDTO.Create(annualFinancialStatement));
            }
        }
    }
}