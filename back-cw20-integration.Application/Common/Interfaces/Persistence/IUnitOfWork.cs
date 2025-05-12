using System.Data;

namespace back_cw20_integration.Application.Common.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        IDbConnection Connection { get; }
        IDbTransaction? CurrentTransaction { get; }
    }
}
