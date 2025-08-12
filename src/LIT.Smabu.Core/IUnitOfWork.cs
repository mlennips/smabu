
namespace LIT.Smabu.Core
{
    public interface IUnitOfWork
    {
        IAggregateRepository Repository { get; }
        bool HasChanges { get; }

        Task CommitAsync(CancellationToken? cancellationToken = null);
    }
}
