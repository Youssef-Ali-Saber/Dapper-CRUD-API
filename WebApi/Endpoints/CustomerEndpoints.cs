using Dapper;
using WebApi.Factory;
using WebApi.Models;

namespace WebApi.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        app.MapGet("customers", async (SqlConnectionFactory sqlConnectionFactory) =>
        {
            using var connection = sqlConnectionFactory.Create();
            const string sql = "SELECT * FROM Customers";
            var customers = await connection.QueryAsync<Customer>(sql);
            return Results.Ok(customers);
        });
    }
}
