using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.Common
{
    public static class CommonErrors
    {
        public static readonly ErrorDetail HasReferences = new("Common.HasReferences", "There are already references to other elements.");
    }
}
