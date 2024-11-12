using LIT.Smabu.Domain.Base;
using MediatR;

namespace LIT.Smabu.UseCases.Base
{
    public interface IQuery<T> : IRequest<Result<T>>
    {
    }
}
