using LIT.Smabu.Domain.Shared;

namespace LIT.Smabu.Domain.Common
{
    public static class CommonErrors
    {
        public static readonly Error HasReferences = new("Common.HasReferences", "There are already references to other elements.");
    }
}
