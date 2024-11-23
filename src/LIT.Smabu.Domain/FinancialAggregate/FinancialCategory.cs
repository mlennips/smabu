using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.FinancialAggregate
{
    public record FinancialCategory : EnumValueObject
    {
        public override string Name => Value switch
        {
            "Revenue" => "Revenue",
            "OtherIncome" => "Other Income",
            "AssetSaleIncome" => "Asset Sale Income",
            "GoodsPurchase" => "Goods Purchase",
            "OperatingCosts" => "Operating Costs",
            "PersonnelExpenses" => "Personnel Expenses",
            "VehicleCosts" => "Vehicle Costs",
            "OfficeSupplies" => "Office Supplies",
            "Depreciation" => "Depreciation",
            "TravelExpenses" => "Travel Expenses",
            "Insurance" => "Insurance",
            "OtherOperatingExpenses" => "Other Operating Expenses",
            _ => "???"
        };

        public FinancialCategory(string value) : base(value)
        {

        }

        // Income
        public static readonly FinancialCategory Revenue = new("Revenue"); // Umsatzeinnahmen: Einnahmen aus dem Verkauf von Waren oder Dienstleistungen.
        public static readonly FinancialCategory OtherIncome = new("OtherIncome"); // Sonstige Einnahmen: Einnahmen aus Nebenaktivitäten, z.B. Provisionen oder Mieteinnahmen.
        public static readonly FinancialCategory AssetSaleIncome = new("AssetSaleIncome"); // Einnahmen aus dem Verkauf von Anlagevermögen: Erlöse aus dem Verkauf von Betriebsvermögen.

        // Expenses
        public static readonly FinancialCategory GoodsPurchase = new("GoodsPurchase"); // Wareneinkauf: Kosten für den Einkauf von Waren, die verkauft werden.
        public static readonly FinancialCategory OperatingCosts = new("OperatingCosts"); // Betriebskosten: Laufende Kosten wie Miete, Strom, Wasser, Heizung.
        public static readonly FinancialCategory PersonnelExpenses = new("PersonnelExpenses"); // Personalaufwand: Löhne und Gehälter für Mitarbeiter sowie Sozialabgaben.
        public static readonly FinancialCategory VehicleCosts = new("VehicleCosts"); // Fahrzeugkosten: Betriebskosten für Fahrzeuge, z.B. Benzin, Wartung und Versicherung.
        public static readonly FinancialCategory OfficeSupplies = new("OfficeSupplies"); // Büromaterial: Kosten für Büromaterialien und -bedarf.
        public static readonly FinancialCategory Depreciation = new("Depreciation"); // Abschreibungen: Wertminderungen auf Anlagegüter (z.B. Maschinen, Computer).
        public static readonly FinancialCategory TravelExpenses = new("TravelExpenses"); // Reisekosten: Kosten für Geschäftsreisen, z.B. Fahrtkosten, Übernachtungen.
        public static readonly FinancialCategory Insurance = new("Insurance"); // Versicherungen: Beiträge für Betriebshaftpflicht, Inventarversicherung etc.
        public static readonly FinancialCategory OtherOperatingExpenses = new("OtherOperatingExpenses"); // Sonstige betriebliche Aufwendungen: z.B. Werbung, Fortbildungskosten, Mitgliedsbeiträge.

        public static FinancialCategory[] GetAllIncomes()
        {
            return [Revenue, OtherIncome, AssetSaleIncome];
        }

        public static FinancialCategory[] GetAllExpenditures()
        {
            return [GoodsPurchase, OperatingCosts, PersonnelExpenses, VehicleCosts, OfficeSupplies, Depreciation, TravelExpenses, Insurance, OtherOperatingExpenses];
        }

        public static bool CheckIsIncome(FinancialCategory category)
        {
            return GetAllIncomes().Contains(category);
        }

        public static bool CheckIsExpenditure(FinancialCategory category)
        {
            return GetAllExpenditures().Contains(category);
        }
    }
}
