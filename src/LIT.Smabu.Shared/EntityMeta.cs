using System.ComponentModel.DataAnnotations.Schema;

namespace LIT.Smabu.Shared
{
    [ComplexType]
    public record EntityMeta : IValueObject
    {
        public DateTime CreatedAt { get; init; }
        public string CreatedById { get; init; }
        public string CreatedByName { get; init; }
        public DateTime? ModifiedAt { get; init; }
        public string? ModifiedById { get; init; }
        public string? ModifiedByName { get; init; }

        public EntityMeta(DateTime createdAt, string createdById, string createdByName, DateTime? modifiedAt, string? modifiedById, string? modifiedByName)
        {
            CreatedAt = createdAt;
            CreatedById = createdById;
            CreatedByName = createdByName;

            if (modifiedAt != null)
            {
                if (string.IsNullOrEmpty(modifiedByName))
                {
                    throw new ArgumentException($"\"{nameof(modifiedByName)}\" kann nicht NULL oder leer sein.", nameof(createdByName));
                }
                ArgumentNullException.ThrowIfNull(modifiedById);

                ModifiedAt = modifiedAt;
                ModifiedById = modifiedById;
                ModifiedByName = modifiedByName;
            }
        }
    }
}
