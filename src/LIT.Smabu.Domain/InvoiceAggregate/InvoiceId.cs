using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.InvoiceAggregate
{
    public record InvoiceId(Guid Value) : EntityId<Invoice>(Value);
}