﻿using LIT.Smabu.UseCases.Offers;
using LIT.Smabu.Web.Pages.Shared.Documents;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LIT.Smabu.Web.Areas.Invoices.Documents
{
    public class OfferDocument(OfferWithItemsDTO offer) : IDocument
    {
        static readonly string senderAddress = "Abs.: M. Lennips - Fennhook 28 - 49828 Veldhausen";

        public void Compose(IDocumentContainer container)
        {
            container
               .Page(page =>
               {
                   page.MarginVertical(Constants.PageMargin);
                   page.MarginHorizontal(Constants.PageMargin);
                   page.DefaultTextStyle(Constants.DefaultTextStyle);

                   page.Header().Component(new HeaderComponent("Angebot"));
                   page.Content().Element(ComposeContent);
                   page.Footer().Component(new FooterComponent());
               });
        }
        
        void ComposeContent(IContainer container)
        {
            container.PaddingTop(80).PaddingBottom(40).PaddingLeft(Constants.PaddingMedium).Column(column =>
            {
                column.Item().DefaultTextStyle(Constants.DefaultTextStyle).Row(row =>
                {
                    row.RelativeItem().Component(new AddressComponent(senderAddress, offer.Customer.MainAddress));
                    row.ConstantItem(20);
                    row.RelativeItem().Component(new InfoBlockComponent(offer.Number.Long, offer.OfferDate, offer.Customer.Number.Long, null));
                });

                column.Item().PaddingTop(40).Element(ComposeTable);

                column.Item().PaddingTop(Constants.PaddingLarge).Row(row =>
                {
                    row.RelativeItem().Element(ComposeComments);
                    row.ConstantItem(20);
                    row.RelativeItem().Element(ComposeTotals);
                });
            });
        }

        void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(4);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("#");
                    header.Cell().Element(CellStyle).Text("Leistung");
                    header.Cell().Element(CellStyle).AlignRight().Text("Preis");
                    header.Cell().Element(CellStyle).AlignRight().Text("Anzahl");
                    header.Cell().Element(CellStyle).BorderRight(0).AlignRight().Text("Gesamt");

                    IContainer CellStyle(IContainer container) => container.BorderRight(1).BorderColor(Colors.White).Background(Constants.AccentColor1Light)
                        .DefaultTextStyle(Constants.DefaultTextStyle.FontColor(Colors.White)).Padding(5).AlignCenter();
                });

                foreach (var item in offer.Items)
                {
                    table.Cell().Element(CellStyle).Text(item.Position.ToString());
                    table.Cell().Element(CellStyle).Text(item.Details);
                    table.Cell().Element(CellStyle).AlignRight().Text($"{offer.Currency.Format(item.UnitPrice)}");
                    table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity.ToString());
                    table.Cell().Element(CellStyle).AlignRight().Text($"{offer.Currency.Format(item.TotalPrice)}");

                    IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten4)
                        .DefaultTextStyle(Constants.DefaultTextStyle).Padding(5);
                }

            });
        }

        void ComposeComments(IContainer container)
        {
            container.Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
            {
                column.Spacing(Constants.PaddingSmall);
                column.Item().Text("Alle angegebenen Preise verstehen sich als Endpreise. Umsatzsteuer wird aufgrund der Befreiung für Kleinunternehmer gemäß §19 Abs. 1 UStG nicht gesondert ausgewiesen.").Light().FontSize(8);
                column.Item().Text("Zahlungsbedingung: 15 Tage - Kein Skonto.").Light().FontSize(8);
                column.Item().Text($"Angebot gültig bis zum {offer.ExpiresOn.ToShortDateString()}").FontSize(8);
            });
        }

        void ComposeTotals(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Cell().Row(1).Column(1).Element(CellStyle1).Text("Zwischensumme");
                    table.Cell().Row(1).Column(2).Element(CellStyle1).Text($"{offer.Currency.Format(offer.Amount)}");

                    table.Cell().Row(2).Column(1).Element(CellStyle1).Text("zzgl. MwSt.");
                    table.Cell().Row(2).Column(2).Element(CellStyle1).Text($"entfällt");

                    table.Cell().Row(3).Column(1).Element(CellStyle1).Text("Gesamtsumme");
                    table.Cell().Row(3).Column(2).Element(CellStyle1).Text($"{offer.Currency.Format(offer.Amount)}");

                    IContainer CellStyle1(IContainer container) => container.Padding(Constants.PaddingSmall).AlignRight();
                });
            });
        }
    }
}
