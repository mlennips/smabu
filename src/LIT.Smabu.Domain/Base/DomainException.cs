using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.Base
{
    public class DomainException(string message, IEntityId? entityId = null) : Exception(BuildMessage(message, entityId))
    {
        private static string BuildMessage(string message, IEntityId? entityId)
        {
            return message + (entityId != null ? $" ({entityId.GetType().Name}: {entityId})" : "");
        }
    }
}
