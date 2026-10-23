using System.Data;
using Microsoft.Data.SqlClient;

namespace PracticalNCode.Infrastructure.Repositories;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
    IDbConnection GetConnection();
}

public sealed class SqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");
        }
        return new SqlConnection(connectionString);
    }

    public IDbConnection GetConnection() => CreateConnection();
}
