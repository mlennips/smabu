using MediatR;
using LIT.Smabu.UseCases.Payments.Get;
using LIT.Smabu.UseCases.Payments.Create;
using LIT.Smabu.UseCases.Payments.Update;
using LIT.Smabu.UseCases.Payments.Delete;
using LIT.Smabu.UseCases.Payments.Complete;
using LIT.Smabu.UseCases.Payments;
using LIT.Smabu.Domain.PaymentAggregate;
using LIT.Smabu.UseCases.Payments.List;
using LIT.Smabu.Domain.Base;

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