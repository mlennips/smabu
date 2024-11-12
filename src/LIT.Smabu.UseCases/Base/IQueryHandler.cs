using LIT.Smabu.Domain.Base;
using MediatR;

namespace LIT.Smabu.UseCases.Base
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    {
    }
}
