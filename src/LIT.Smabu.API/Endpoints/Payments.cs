using MediatR;
using LIT.Smabu.UseCases.Payments;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.Domain.Base;
using static LIT.Smabu.UseCases.Payments.ListPayments;
using static LIT.Smabu.UseCases.Payments.GetPayment;
using static LIT.Smabu.UseCases.Payments.CreatePayment;
using static LIT.Smabu.UseCases.Payments.DeletePayment;
using static LIT.Smabu.UseCases.Payments.UpdatePayment;
using static LIT.Smabu.UseCases.Payments.CompletePayment;

namespace LIT.Smabu.API.Endpoints
{
    public static class Payments
    {
        public static void RegisterPaymentsEndpoints(this IEndpointRouteBuilder routes)
        {
            var api = routes.MapGroup("/payments")
                .WithTags(["Payments"])
                .RequireAuthorization();

            AddPayments(api);
        }

        private static void AddPayments(RouteGroupBuilder api)
        {
            api.MapGet("", async (IMediator mediator) =>
                await mediator.SendAndMatchAsync(new ListPaymentsQuery(),
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<PaymentDTO[]>();

            api.MapGet("/{paymentId}", async (IMediator mediator, Guid paymentId) =>
                await mediator.SendAndMatchAsync(new GetPaymentQuery(new(paymentId)),
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<PaymentDTO>();

            api.MapPost("", async (IMediator mediator, CreatePaymentCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces<PaymentId>();

            api.MapPut("/{paymentId}", async (IMediator mediator, Guid paymentId,
                UpdatePaymentCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200);

            api.MapPut("/{paymentId}/complete", async (IMediator mediator, Guid paymentId,
                CompletePaymentCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200);

            api.MapDelete("/{paymentId}", async (IMediator mediator, Guid paymentId) =>
                await mediator.SendAndMatchAsync(new DeletePaymentCommand(new(paymentId)),
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200)
                .Produces<ErrorDetail>(400);
        }
    }
}