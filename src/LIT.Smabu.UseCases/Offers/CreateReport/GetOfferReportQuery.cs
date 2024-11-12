using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Shared;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Offers.CreateReport
{
    public record GetOfferReportQuery(OfferId OfferId) : IQuery<IReport>
    {

    }
}