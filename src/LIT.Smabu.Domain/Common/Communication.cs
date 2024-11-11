using LIT.Smabu.Shared;

namespace LIT.Smabu.Domain.Common
{
    public record Communication : IValueObject
    {
        public string Email { get; private set; }
        public string Mobil { get; private set; }
        public string Phone { get; private set; }
        public string Website { get; private set; }

        public Communication(string email, string mobil, string phone, string website)
        {
            Email = email;
            Mobil = mobil;
            Phone = phone;
            Website = website;
        }

        public static Communication Empty => new("", "", "", "");
    }
}
