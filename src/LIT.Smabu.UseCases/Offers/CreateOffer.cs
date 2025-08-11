using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Domain.Services;
using LIT.Smabu.Core;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Offers
{
    public static class CreateOffer
    {
        public record CreateOfferCommand(OfferId OfferId, CustomerId CustomerId, Currency Currency, TaxRate? TaxRate, OfferNumber? Number) : ICommand<OfferId>;

        public class CreateOfferHandler(IAggregateRepository repository, BusinessNumberService businessNumberService) : ICommandHandler<CreateOfferCommand, OfferId>
        {
            public async Task<Result<OfferId>> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
            {
                Customer customer = await repository.GetByAsync(request.CustomerId);
                OfferNumber number = request.Number ?? await businessNumberService.CreateOfferNumberAsync();
                var offer = Offer.Create(request.OfferId, request.CustomerId, number, customer.MainAddress,
                    request.Currency, request.TaxRate ?? TaxRate.Default);
                await repository.CreateAsync(offer);
                return offer.Id;
            }
        }
    }
}