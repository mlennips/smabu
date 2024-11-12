using LIT.Smabu.Domain.Base;
using MediatR;

namespace LIT.Smabu.UseCases.Shared
{
    public interface IQuery<T> : IRequest<Result<T>>
    {
    }
}
