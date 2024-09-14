using LIT.Smabu.Domain.Common;
using LIT.Smabu.Domain.OfferAggregate;
using LIT.Smabu.UseCases.Offers;
using LIT.Smabu.Web.Areas.Offers.Documents;
using LIT.Smabu.Web.Pages.Shared.Documents;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuestPDF.Fluent;

namespace LIT.Smabu.Web.Areas.Offers.Pages
{
    public class DetailsModel(IMediator mediator) : PageModel
    {
        [BindProperty]
        public Guid Id { get; set; }
        [BindProperty]
        public string Number { get; set; }
        [BindProperty]
        public decimal Tax { get; set; }
        [BindProperty]
        public string TaxDetails { get; set; }

        public string Customer { get; private set; }
        public string DisplayName { get; private set; }
        public Currency Currency { get; private set; }
        public List<OfferItemDTO> Items { get; private set; }

        public async Task OnGetAsync(Guid id)
        {
            var offer = await mediator.Send(new UseCases.Offers.Get.GetOfferQuery(new OfferId(id)) { WithItems = true });
            Id = offer.Id.Value;
            Number = offer.Number.Long;
            Tax = offer.Tax;
            TaxDetails = offer.TaxDetails;
            Customer = offer.Customer.DisplayName;
            DisplayName = offer.DisplayName;
            Currency = offer.Currency;
            Items = offer.Items;
        }

        public async Task<IActionResult> OnPostDownloadPDFAsync(Guid id)
        {
            var offer = await mediator.Send(new UseCases.Offers.Get.GetOfferQuery(new(id)) { WithItems = true });
            var offerDocument = new OfferDocument(offer);
            return File(offerDocument.GeneratePdf(), "application/pdf", Utils.CreateFileNamePDF(offer));
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var result = await mediator.Send(new UseCases.Offers.Update.UpdateOfferCommand()
            {
                Id = new OfferId(id),
                Tax = Tax,
                TaxDetails = TaxDetails
            });

            if(result != null)
            {
                return RedirectToPage(new { id });
            }

            return Page();
        }
    }
}
