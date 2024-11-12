using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.UseCases.Shared;

namespace LIT.Smabu.UseCases.Payments.Complete
{
    public record CompletePaymentCommand(PaymentId PaymentId, decimal Amount, DateTime PaidAt) : ICommand
    {

    }
}
