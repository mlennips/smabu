using LIT.Smabu.Domain.Shared;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Offers.CreateReport
{
    public class GetInvoiceReportHandler(IReportFactory reportFactory) : IQueryHandler<GetOfferReportQuery, IReport>
    {
        public async Task<Result<IReport>> Handle(GetOfferReportQuery request, CancellationToken cancellationToken)
        {
            var report = await reportFactory.CreateOfferReportAsync(request.OfferId);
            return Result<IReport>.Success(report);
        }
    }
}
