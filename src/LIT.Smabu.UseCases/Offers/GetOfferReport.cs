using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Offers
{
    public static class GetOfferReport
    {
        public record GetOfferReportQuery(OfferId OfferId) : IQuery<IReport>;

        public class GetInvoiceReportHandler(IReportFactory reportFactory) : IQueryHandler<GetOfferReportQuery, IReport>
        {
            public async Task<Result<IReport>> Handle(GetOfferReportQuery request, CancellationToken cancellationToken)
            {
                var report = await reportFactory.CreateOfferReportAsync(request.OfferId);
                return Result.Success(report);
            }
        }
    }
}