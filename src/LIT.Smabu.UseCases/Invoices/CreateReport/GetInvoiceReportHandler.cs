﻿using LIT.Smabu.Domain.Shared;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Invoices.CreateReport
{
    public class GetInvoiceReportHandler(IReportFactory reportFactory) : IQueryHandler<GetInvoiceReportQuery, IReport>
    {
        public async Task<Result<IReport>> Handle(GetInvoiceReportQuery request, CancellationToken cancellationToken)
        {
            var report = await reportFactory.CreateInvoiceReportAsync(request.Id);
            return Result<IReport>.Success(report);
        }
    }
}
