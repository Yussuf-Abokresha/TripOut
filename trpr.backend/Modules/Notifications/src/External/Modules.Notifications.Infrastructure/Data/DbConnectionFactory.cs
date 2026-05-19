using System.Data.Common;
using Modules.Notifications.Application.Abstractions;
using Npgsql;

namespace Modules.Notifications.Infrastructure.Data
{
    public class DbConnectionFactory(string ConnectionString) : IDbConnectionFactory
    {
        public async Task<DbConnection> CreateSqlConnection()
        {
            var connection = new NpgsqlConnection(ConnectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}