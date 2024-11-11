using LIT.Smabu.Domain.Shared;
using MediatR;

namespace LIT.Smabu.UseCases.Shared
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    {
    }
}
