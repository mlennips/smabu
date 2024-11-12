using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.Domain.PaymentAggregate
{
    public record PaymentId(Guid Value)  : EntityId<Payment>(Value)
    {

    }
}