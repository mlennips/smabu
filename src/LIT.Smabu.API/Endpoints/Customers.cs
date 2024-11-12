using LIT.Smabu.UseCases.Customers.Create;
using LIT.Smabu.UseCases.Customers.Delete;
using LIT.Smabu.UseCases.Customers.Update;
using MediatR;
using LIT.Smabu.UseCases.Customers.List;
using LIT.Smabu.UseCases.Customers.Get;
using LIT.Smabu.UseCases.Customers;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.Domain.Base;

namespace LIT.Smabu.API.Endpoints
{
    public static class Customers
    {
        public static void RegisterCustomersEndpoints(this IEndpointRouteBuilder routes)
        {
            var api = routes.MapGroup("/customers")
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
