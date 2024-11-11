
namespace LIT.Smabu.Domain.Common
{
    public record CorporateDesign
    {

        public string Brand { get; private set; }
        public string ShortName { get; private set; }
        public string? Slogan { get; private set; }
        public Color Color1 { get; private set; }
        public Color Color2 { get; private set; }
        public FileReference? Logo { get; private set; }

        public CorporateDesign(string brand, string shortName, string? slogan, Color color1, Color color2, FileReference? logo)
        {
            Brand = brand;
            ShortName = shortName;
            Slogan = slogan;
            Color1 = color1;
            Color2 = color2;
            Logo = logo;
        }

        internal static CorporateDesign CreateDefault(string name)
        {
            var color1 = Color.Create("#4682b4");
            var color2 = Color.CreateComplementary(color1.Hex);
            return new CorporateDesign(name, SuggestShortName(name), null, color1, color2, null);
        }

        public static string SuggestShortName(string name)
        {
            var formattedName = new string(name.Where(char.IsLetterOrDigit).ToArray()).Replace(" ", "");
            return formattedName.Length > 8
                ? formattedName[..8].ToUpper()
                : formattedName.ToUpper();
        }
    }
}
