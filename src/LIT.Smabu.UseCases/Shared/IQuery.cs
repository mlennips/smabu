using LIT.Smabu.Domain.Shared;
using MediatR;

namespace LIT.Smabu.UseCases.Shared
{
    public interface IQuery<T> : IRequest<Result<T>>
    {
    }
}
