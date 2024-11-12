using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.Domain.OfferAggregate.Services;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Offers
{
    public static class DeleteOffer
    {
        public record DeleteOfferCommand(OfferId OfferId) : ICommand;

        public class DeleteOfferHandler(DeleteOfferService deleteOfferService) : ICommandHandler<DeleteOfferCommand>
        {
            public async Task<Result> Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
            {
                var result = await deleteOfferService.DeleteAsync(request.OfferId);
                return result;
            }
        }
    }
}