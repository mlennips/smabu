using LIT.Smabu.Core;
using LIT.Smabu.Domain.Base;
using MediatR;

namespace LIT.Smabu.UseCases.Base
{
    public interface ICommand<T> : ICommandBase, IRequest<Result<T>>
    {
    }

    public interface ICommand : ICommandBase, IRequest<Result>
    {
    }

    public interface  ICommandBase : IRequest
    {
        
    }
}
