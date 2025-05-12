using back_cw20_integration.Application.Common.Interfaces.Persistence;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace back_cw20_integration.Infrastructure.Repositories
{
    public abstract class RepositoryBase(IUnitOfWork unitOfWork)
    {
        protected readonly IUnitOfWork UnitOfWork = unitOfWork;

        protected async Task BullkInsertAsync(string tempTableName, DataTable data, CancellationToken cancellationToken = default)
        {
            using var bulkCopy = new SqlBulkCopy((SqlConnection) UnitOfWork.Connection);
            bulkCopy.DestinationTableName = tempTableName;

            foreach (DataColumn col in data.Columns)
            {
                bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
            }

            await bulkCopy.WriteToServerAsync(data, cancellationToken);
        }

        protected async Task<int> ExecuteAsync(string sql, CommandType commandType, object? parameters = null, CancellationToken cancellationToken = default)
        {
            return await UnitOfWork.Connection.ExecuteAsync(
                new CommandDefinition(
                    commandText: sql,
                    commandType: commandType,
                    parameters: parameters,
                    transaction: UnitOfWork.CurrentTransaction,
                    cancellationToken: cancellationToken
                )
            );
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, CommandType commandType, object? parameters = null, CancellationToken cancellationToken = default)
        {
            return await UnitOfWork.Connection.QueryAsync<T>(
                new CommandDefinition(
                    commandText: sql,
                    parameters: parameters,
                    commandType: commandType,
                    transaction: UnitOfWork.CurrentTransaction,
                    cancellationToken: cancellationToken
                )
            );
        }

        protected async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, CommandType commandType, object? parameters = null, CancellationToken cancellationToken = default)
        {
            return await UnitOfWork.Connection.QueryFirstOrDefaultAsync<T>(
                new CommandDefinition(
                    commandText: sql,
                    commandType: commandType,
                    parameters: parameters,
                    transaction: UnitOfWork.CurrentTransaction,
                    cancellationToken: cancellationToken
                )
            );
        }
    }
}
