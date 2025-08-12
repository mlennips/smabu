using LIT.Smabu.Core;
using LIT.Smabu.Domain.CustomerAggregate;
using LIT.Smabu.UseCases.Base;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIT.Smabu.Infrastructure.Persistence
{
    public class UnitOfWorkBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork) 
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next(cancellationToken);

            var t1 = typeof(TRequest);  
            var t2 = typeof(TResponse);
            var t3 = t1.Name + " " + t2.Name;

            if (unitOfWork.HasChanges)
            {
                if (response is Result result && result.IsSuccess)
                {
                    // If the response is a Result and indicates success, we commit the changes.
                    await unitOfWork.CommitAsync(cancellationToken);
                }
                else if (response is Result resultWithError && resultWithError.IsFailure)
                {
                    // If the response is a Result with an error, we throw an exception
                    // to indicate that the commit failed due to an unsuccessful operation.
                    throw new InvalidOperationException($"Unit of Work commit failed due to an unsuccessful result: {resultWithError.Error.Description}");
                }
                else
                {
                    // If the response is not a Result, we assume it is a command that has been handled successfully.
                    // We can commit the changes without checking for success.
                    await unitOfWork.CommitAsync(cancellationToken);
                }
            }

            return response;
        }
    }

    public class VoidRequestValidationBehavior<TRequest, TResponse>() : IPipelineBehavior<TRequest, MediatR.Unit>
    where TRequest : notnull
    {
        public async Task<Unit> Handle(TRequest request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
        {
            // Validate request
            //var context = new ValidationContext<TRequest>(request);
            //var failures = (await Task.WhenAll(validators
            //        .Select(v => v.ValidateAsync(context, cancellationToken))))
            //    .SelectMany(result => result.Errors)
            //    .Where(error => error != null)
            //    .ToList();

            //if (failures.Any())
            //    throw new AppValidationException(failures);

            return await next();
        }
    }
}
