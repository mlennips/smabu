using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Payments.Delete
{
    public record DeletePaymentCommand(PaymentId PaymentId) : ICommand
    {
    }
}
