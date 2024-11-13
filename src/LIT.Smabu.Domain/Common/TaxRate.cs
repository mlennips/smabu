namespace LIT.Smabu.Domain.Common
{
    public record TaxRate
    {
        public string Name { get; init; }
        public decimal Rate { get; init; }
        public string Details { get; init; }

        public TaxRate(string name, decimal rate, string details)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
            if (string.IsNullOrWhiteSpace(details))
                throw new ArgumentException("Details cannot be null or whitespace.", nameof(details));
            if (rate < 0)
                throw new ArgumentException("Rate cannot be smaller than 0.", nameof(rate));

            Name = name;
            Rate = rate;
            Details = details;
        }

        public static TaxRate Default => new("Kleinunternehmer Steuerbefreit", 0, "Umsatzsteuer wird aufgrund der Befreiung für Kleinunternehmer gemäß § 19 Abs. 1 UStG nicht gesondert ausgewiesen.");

        public static TaxRate[] GetAll() => [Default];
    }
}
