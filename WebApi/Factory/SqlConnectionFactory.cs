using Microsoft.Data.SqlClient;

namespace WebApi.Factory;

public class SqlConnectionFactory(string connectionString)
{
    public SqlConnection Create()
    {
        return new SqlConnection(connectionString);
    }
}
