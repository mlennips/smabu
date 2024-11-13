using LIT.Smabu.Core;

namespace LIT.Smabu.Domain.InvoiceAggregate.Events
{
    public record InvoiceReleasedEvent(InvoiceId InvoiceId) : IDomainEvent
    {

    }
}
