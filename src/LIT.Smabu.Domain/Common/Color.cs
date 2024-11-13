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
            var rgbValue = GetRGB(hex);
            var r = (rgbValue >> 16) & 0xFF;
            var g = (rgbValue >> 8) & 0xFF;
            var b = rgbValue & 0xFF;

            var luminance = (int)(0.299 * r + 0.587 * g + 0.114 * b);

            var complementaryColor = luminance > 128 ? "#000000" : "#FFFFFF";
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
