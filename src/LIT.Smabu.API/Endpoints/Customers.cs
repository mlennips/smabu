﻿using LIT.Smabu.UseCases.Customers.Create;
using LIT.Smabu.UseCases.Customers.Delete;
using LIT.Smabu.UseCases.Customers.Update;
using LIT.Smabu.API;
using MediatR;

namespace LIT.Smabu.API.Endpoints
{
    public static class Customers
    {
        public static void RegisterCustomersEndpoints(this IEndpointRouteBuilder routes)
        {
            var api = routes.MapGroup("/customers")
                .WithTags(["Customers"])
                .RequireAuthorization();

            api.MapPost("/", async (IMediator mediator, CreateCustomerCommand command) => await mediator.SendAndMatchAsync(command));
            api.MapGet("/", async (IMediator mediator) => await mediator.Send(new UseCases.Customers.List.ListCustomersQuery()));
            api.MapGet("/{id}", async (IMediator mediator, Guid id) => await mediator.Send(new UseCases.Customers.Get.GetCustomerQuery(new(id))));
            api.MapPut("/{id}", async (IMediator mediator, Guid id, UpdateCustomerCommand command) => await mediator.SendAndMatchAsync(command));
            api.MapDelete("/{id}", async (IMediator mediator, Guid id) => (await mediator.SendAndMatchAsync(new DeleteCustomerCommand(new(id)))));
        }
    }
}
