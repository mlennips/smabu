using LIT.Smabu.Core;

namespace LIT.Smabu.UseCases.Orders
{
    public record OrderReferenceDTO<TId>(TId Id, string Name, bool? IsSelected, DateOnly? Date, decimal? Amount) where TId : IEntityId
    {

    }
}