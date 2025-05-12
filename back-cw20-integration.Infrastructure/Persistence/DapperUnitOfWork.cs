using back_cw20_integration.Application.Common.Interfaces.Persistence;
using System.Data;

namespace back_cw20_integration.Infrastructure.Persistence
{
    public class DapperUnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        private IDbTransaction? _transaction;
        private bool _disposed;

        public DapperUnitOfWork(IDbConnectionFactory connectionFactory) 
        { 
            _connection = connectionFactory.CreateConnection();
            _connection.Open();
        }

        public IDbConnection Connection => _connection;
        public IDbTransaction? CurrentTransaction => _transaction;

        public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null) return Task.CompletedTask;

            _transaction = _connection.BeginTransaction();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
                throw new InvalidOperationException("No hay una transacción activa para hacer commit");

            try
            {
                _transaction.Commit();
                return Task.CompletedTask;
            }
            catch
            {
                RollbackTransactionAsync(cancellationToken).GetAwaiter().GetResult();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }

                _connection?.Dispose();

                _disposed = true;
            }
        }

        public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
                return Task.CompletedTask;

            try
            {
                _transaction.Rollback();
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }

            return Task.CompletedTask;
        }
    }
}
