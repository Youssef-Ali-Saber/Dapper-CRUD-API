using Dapper;
using WebApi.Factory;
using WebApi.Models;

namespace WebApi.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/customers")
            .WithTags("Customers");

        group.MapGet("", async (SqlConnectionFactory sqlConnectionFactory) =>
        {
            using var connection = sqlConnectionFactory.Create();
            const string sql = "SELECT * FROM Customers";
            var customers = await connection.QueryAsync<Customer>(sql);
            return Results.Ok(customers);
        });

        group.MapGet("{id}", async (int id, SqlConnectionFactory sqlConnectionFactory) =>
        {
            using var connection = sqlConnectionFactory.Create();
            const string sql = "SELECT * FROM Customers WHERE Id = @Id";
            var customer = await connection.QuerySingleOrDefaultAsync<Customer>(sql, new { Id = id });
            return customer is not null ? Results.Ok(customer) : Results.NotFound();
        });

        group.MapPost("", async (Customer customer, SqlConnectionFactory sqlConnectionFactory) =>
        {
            using var connection = sqlConnectionFactory.Create();
            const string sql = @"
                INSERT INTO Customers (Id, FirstName, LastName, Email, DateOfBirth)
                VALUES (@Id, @FirstName, @LastName, @Email, @DateOfBirth);";
            await connection.ExecuteAsync(sql, customer);
            return Results.Created();
        });

        group.MapPut("{id}", async (int id, Customer customer, SqlConnectionFactory sqlConnectionFactory) =>
        {
            using var connection = sqlConnectionFactory.Create();
            customer.Id = id;
            const string sql = @"
                UPDATE Customers
                SET FirstName = @FirstName, LastName = @LastName, Email = @Email, DateOfBirth = @DateOfBirth
                WHERE Id = @Id;";
            await connection.ExecuteAsync(sql, customer);
            return Results.NoContent();
        });
    }
}
