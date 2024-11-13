using System.ComponentModel.DataAnnotations.Schema;

namespace LIT.Smabu.Core
{
    [ComplexType]
    public record AggregateMeta : EntityMeta
    {
        public long Version { get; init; }

        public AggregateMeta(long version, DateTime createdAt, string createdById, string createdByName, DateTime? modifiedAt, string? modifiedById, string? modifiedByName)
            : base(createdAt, createdById, createdByName, modifiedAt, modifiedById, modifiedByName)
        {
            Version = version;
        }

        public static AggregateMeta CreateLegacy(ICurrentUser currentUser, DateTime createdAt)
        {
            return new AggregateMeta(1, createdAt, currentUser.Username, currentUser.Name, DateTime.Now, currentUser.Username, currentUser.Name);
        }

        public static AggregateMeta CreateFirst(ICurrentUser currentUser)
        {
            return new AggregateMeta(1, DateTime.Now, currentUser.Username, currentUser.Name, null, null, null);
        }

        public AggregateMeta Next(ICurrentUser currentUser)
        {
            return new AggregateMeta(Version + 1, CreatedAt, CreatedById, CreatedByName,
                DateTime.Now, currentUser.Username, currentUser.Name);
        }
    }
}
