using LIT.Smabu.Domain.Base;
using LIT.Smabu.Domain.OfferAggregate.Services;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Offers.Delete
{
    public class DeleteOfferHandler(DeleteOfferService deleteOfferService) : ICommandHandler<DeleteOfferCommand>
    {
        public async Task<Result> Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
        {
            var result = await deleteOfferService.DeleteAsync(request.OfferId);
            return result; 
        }
    }
}
