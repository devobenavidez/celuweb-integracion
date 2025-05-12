using System.Data;

namespace back_cw20_integration.Application.Common.Interfaces.Persistence
{
    public  interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
