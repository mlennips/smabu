using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.InvoiceAggregate
{
    public record InvoiceItemId(Guid Value) : EntityId<InvoiceItem>(Value);
}