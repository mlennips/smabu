using MediatR;
using LIT.Smabu.UseCases.Customers;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.Base;
using static LIT.Smabu.UseCases.Customers.CreateCustomer;
using static LIT.Smabu.UseCases.Customers.ListCustomer;
using static LIT.Smabu.UseCases.Customers.GetCustomer;
using static LIT.Smabu.UseCases.Customers.UpdateCustomer;
using static LIT.Smabu.UseCases.Customers.DeleteCustomer;

namespace LIT.Smabu.API.Endpoints
{
    public static class Customers
    {
        public static void RegisterCustomersEndpoints(this IEndpointRouteBuilder routes)
        {
            RouteGroupBuilder api = routes.MapGroup("/customers")
                .WithTags(["Customers"])
                .RequireAuthorization();

            api.MapPost("/", async (IMediator mediator, CreateCustomerCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<CustomerId>();

            api.MapGet("/", async (IMediator mediator) =>
                await mediator.SendAndMatchAsync(new ListCustomersQuery(),
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<CustomerDTO[]>();

            api.MapGet("/{customerId}", async (IMediator mediator, Guid customerId) =>
                await mediator.SendAndMatchAsync(new GetCustomerQuery(new(customerId)),
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<CustomerDTO>();

            api.MapPut("/{customerId}", async (IMediator mediator, Guid customerId, UpdateCustomerCommand command) =>
                await mediator.SendAndMatchAsync(command,
                    onSuccess: Results.Ok,
                    onFailure: Results.BadRequest))
                .Produces<CustomerId>();

            api.MapDelete("/{customerId}", async (IMediator mediator, Guid customerId) =>
                await mediator.SendAndMatchAsync(new DeleteCustomerCommand(new(customerId)),
                    onSuccess: () => Results.Ok(),
                    onFailure: Results.BadRequest))
                .Produces(200)
                .Produces<ErrorDetail>(400);
        }
    }
}
