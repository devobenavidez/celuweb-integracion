using back_cw20_integration.Application.Common.Interfaces.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace back_cw20_integration.Infrastructure.Persistence
{
    public class SqlConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
    {
        private readonly string _connectionString = configuration.GetConnectionString("MAIN") 
            ?? throw new ArgumentNullException("La cadena de conexión no puede ser nula");
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
