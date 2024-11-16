using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.FinancialAggregate
{
    public record FinancialCategory : SimpleValueObject<string>
    {
        public FinancialCategory(string value) : base(value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException("The financial category must not be empty.");
            }
        }

        // Income
        public static readonly FinancialCategory Revenue = new("Revenue"); // Umsatzeinnahmen: Einnahmen aus dem Verkauf von Waren oder Dienstleistungen.
        public static readonly FinancialCategory OtherIncome = new("Other Income"); // Sonstige Einnahmen: Einnahmen aus Nebenaktivitäten, z.B. Provisionen oder Mieteinnahmen.
        public static readonly FinancialCategory AssetSaleIncome = new("Asset Sale Income"); // Einnahmen aus dem Verkauf von Anlagevermögen: Erlöse aus dem Verkauf von Betriebsvermögen.

        // Expenses
        public static readonly FinancialCategory GoodsPurchase = new("Goods Purchase"); // Wareneinkauf: Kosten für den Einkauf von Waren, die verkauft werden.
        public static readonly FinancialCategory OperatingCosts = new("Operating Costs"); // Betriebskosten: Laufende Kosten wie Miete, Strom, Wasser, Heizung.
        public static readonly FinancialCategory PersonnelExpenses = new("Personnel Expenses"); // Personalaufwand: Löhne und Gehälter für Mitarbeiter sowie Sozialabgaben.
        public static readonly FinancialCategory VehicleCosts = new("Vehicle Costs"); // Fahrzeugkosten: Betriebskosten für Fahrzeuge, z.B. Benzin, Wartung und Versicherung.
        public static readonly FinancialCategory OfficeSupplies = new("Office Supplies"); // Büromaterial: Kosten für Büromaterialien und -bedarf.
        public static readonly FinancialCategory Depreciation = new("Depreciation"); // Abschreibungen: Wertminderungen auf Anlagegüter (z.B. Maschinen, Computer).
        public static readonly FinancialCategory TravelExpenses = new("Travel Expenses"); // Reisekosten: Kosten für Geschäftsreisen, z.B. Fahrtkosten, Übernachtungen.
        public static readonly FinancialCategory Insurance = new("Insurance"); // Versicherungen: Beiträge für Betriebshaftpflicht, Inventarversicherung etc.
        public static readonly FinancialCategory OtherOperatingExpenses = new("Other Operating Expenses"); // Sonstige betriebliche Aufwendungen: z.B. Werbung, Fortbildungskosten, Mitgliedsbeiträge.

        public static FinancialCategory[] GetAllIncome()
        {
            return [Revenue, OtherIncome, AssetSaleIncome];
        }

        public static FinancialCategory[] GetAllExpenses()
        {
            return [GoodsPurchase, OperatingCosts, PersonnelExpenses, VehicleCosts, OfficeSupplies, Depreciation, TravelExpenses, Insurance, OtherOperatingExpenses];
        }

        public static bool CheckIsIncome(FinancialCategory category)
        {
            return GetAllIncome().Contains(category);
        }

        public static bool CheckIsExpense(FinancialCategory category)
        {
            return GetAllExpenses().Contains(category);
        }
    }
}
