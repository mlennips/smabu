using LIT.Smabu.Core;
using System.Globalization;

namespace LIT.Smabu.Domain.Common
{
    public record Color : IValueObject
    {
        public string Hex { get; }

        public Color(string hex)
        {
            if (!IsValidHexColor(hex))
            {
                throw new ArgumentException("Invalid hex color value.");
            }
            Hex = hex;
        }

        public static Color Create(string hex)
        {
            return new Color(hex);
        }

        public static Color CreateComplementary(string hex)
        {
            int rgbValue = GetRGB(hex);
            int r = (rgbValue >> 16) & 0xFF;
            int g = (rgbValue >> 8) & 0xFF;
            int b = rgbValue & 0xFF;

            int luminance = (int)(0.299 * r + 0.587 * g + 0.114 * b);

            string complementaryColor = luminance > 128 ? "#000000" : "#FFFFFF";
            return new Color(complementaryColor);
        }

        private static bool IsValidHexColor(string value)
        {
            return !string.IsNullOrEmpty(value) && value.StartsWith('#');
        }

        private static int GetRGB(string hexValue)
        {
            var hexWithoutHash = hexValue.TrimStart('#');
            return int.Parse(hexWithoutHash, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }
    }
}
