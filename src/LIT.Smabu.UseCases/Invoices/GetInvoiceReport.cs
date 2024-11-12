using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.InvoiceAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Invoices
{
    public static class GetInvoiceReport
    {
        public record GetInvoiceReportQuery(InvoiceId InvoiceId) : IQuery<IReport>;
        public class GetInvoiceReportHandler(IReportFactory reportFactory) : IQueryHandler<GetInvoiceReportQuery, IReport>
        {
            public async Task<Result<IReport>> Handle(GetInvoiceReportQuery request, CancellationToken cancellationToken)
            {
                var report = await reportFactory.CreateInvoiceReportAsync(request.InvoiceId);
                return Result.Success(report);
            }
        }
    }
}
