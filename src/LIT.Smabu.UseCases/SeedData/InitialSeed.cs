using LIT.Smabu.Domain.CatalogAggregate;
using LIT.Smabu.Domain.Common;
using LIT.Smabu.Core;

namespace LIT.Smabu.UseCases.SeedData
{
    public class InitialSeed(IAggregateStore store)
    {
        public async Task StartAsync()
        {
            await SeedDefaultCatalogAsync();
            //await SeedTermsOfPaymentsAsync();
        }

        private async Task SeedDefaultCatalogAsync()
        {
            IReadOnlyList<Catalog> items = await store.GetAllAsync<Catalog>();
            if (!items.Any())
            {
                var catalog = Catalog.Create(CatalogId.DefaultId, "Default Catalog");

                var itemNrIdx = 0;

                CatalogGroup group = catalog.AddGroup(new CatalogGroupId(Guid.NewGuid()), "Web Development", "Web Development Services").Value!;
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Custom Website", "", Unit.Project);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "CMS Implementation", "", Unit.Project);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Landing Page", "", Unit.Project);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Service (hourly)", "", Unit.Hour);

                group = catalog.AddGroup(new CatalogGroupId(Guid.NewGuid()), "Software Engineering", "Software Engineering Services").Value!;
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Requirements Analysis", "", Unit.Project);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Architectual Design", "", Unit.Project);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Single Page Application (SPA)", "", Unit.Project);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Progressive Web App (PWA)", "", Unit.Project);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Desktop Application", "", Unit.Project);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Backend/API Development", "", Unit.Project);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Service (hourly)", "", Unit.Hour);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Service (daily)", "", Unit.Day);

                group = catalog.AddGroup(new CatalogGroupId(Guid.NewGuid()), "Hosting", "Hosting Services").Value!;
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Webspace, Datenbank und 1x Domain (SSL)", "", Unit.Project);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Single Domain inkl. SSL", "", Unit.Item);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "E-Mail Account", "", Unit.Item);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "E-Mail (Weiterleitung)", "", Unit.Item);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "CMS Updates", "", Unit.Hour);

                group = catalog.AddGroup(new CatalogGroupId(Guid.NewGuid()), "Maintenance and Support", "Maintenance and Support Services").Value!;
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "CMS Updates", "", Unit.Hour);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "SEO Service", "", Unit.Hour);

                group = catalog.AddGroup(new CatalogGroupId(Guid.NewGuid()), "Consultation and Strategy", "Consultation and Strategy Services").Value!;
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Technical Consultation", "", Unit.Hour);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Digital Strategy", "", Unit.Hour);

                group = catalog.AddGroup(new CatalogGroupId(Guid.NewGuid()), "Specialized Services", "Specialized Services").Value!;
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Integration Services", "", Unit.Hour);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Cloud Services", "", Unit.Hour);

                group = catalog.AddGroup(new CatalogGroupId(Guid.NewGuid()), "Training and Workshops", "Training and Workshops").Value!;
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "CMS Ersteinführung für Benutzer", "", Unit.Hour);

                group = catalog.AddGroup(new CatalogGroupId(Guid.NewGuid()), "Common", "Common Services").Value!;
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Information", "", Unit.Hour);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Hinweis", "", Unit.Hour);
                group.AddItem(new CatalogItemId(Guid.NewGuid()), new CatalogItemNumber(++itemNrIdx), "Rabatt", "", Unit.Hour);

                await store.CreateAsync(catalog);
            }
        }

        //private async Task SeedTermsOfPaymentsAsync()
        //{
        //    var items = await store.GetAllAsync<TermsOfPayment>();
        //    if (!items.Any())
        //    {
        //        var item1 = TermsOfPayment.Create(new(Guid.Parse("89E12676-A882-4B7C-B18F-F8C3D9A484E8")), "15 Tage Netto", "Zahlbar innerhalb von 15 Tagen ohne Abzüge.", 15);
        //        await store.CreateAsync(item1);

        //        var item2 = TermsOfPayment.Create(new(Guid.Parse("DB9DA680-98E5-4AD0-A25A-20FBD6395449")), "Vorkasse", "Zahlbar sofort.", null);
        //        await store.CreateAsync(item2);
        //    }
        //}
    }
}
