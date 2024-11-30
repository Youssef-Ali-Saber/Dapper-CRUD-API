using WebApi.Endpoints;
using WebApi.Factory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(serviceOptions =>
{
    var configuration = serviceOptions.GetRequiredService<IConfiguration>();

    var connectionString = configuration.GetConnectionString("DefaultConnection")
                        ?? throw new ApplicationException("Connection string is missing");

    return new SqlConnectionFactory(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCustomerEndpoints();

app.Run();

