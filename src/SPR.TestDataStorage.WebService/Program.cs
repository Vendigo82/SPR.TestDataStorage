using HealthChecks.UI.Client;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using SPR.TestDataStorage.Infra.Data;
using SPR.TestDataStorage.WebService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SPRTestDataStorageContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

builder.Services.AddHealthChecks();
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("PostgreSQL"));

builder.Services.AddUseCases();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseProblemDetails();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

public class Startup { }
