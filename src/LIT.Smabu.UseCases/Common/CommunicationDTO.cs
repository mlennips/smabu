using LIT.Smabu.Domain.Common;
using LIT.Smabu.UseCases.Base;

namespace LIT.Smabu.UseCases.Common
{
    public record CommunicationDTO(string Email, string Mobil, string Phone, string Website) : IDTO
    {
        public string DisplayName => "";

        public static CommunicationDTO Create(Communication communication)
        {
            return new CommunicationDTO(communication.Email, communication.Mobil, communication.Phone, communication.Website);
        }
    }
}
