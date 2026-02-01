using AuthService.API.Middlewares;
using AuthService.Application;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplication().AddInfrastructure(builder.Configuration);

var app = builder.Build();
// Apply migrations with retry
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<AuthDbContext>();
        const int maxAttempts = 10;
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                context.Database.Migrate();
                logger.LogInformation("Database migrations applied.");
                break;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Database migrate attempt {Attempt}/{MaxAttempts} failed. Retrying in 5s...", attempt, maxAttempts);
                if (attempt == maxAttempts) throw;
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
    }
    catch (Exception ex)
    {
        var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
        loggerFactory.CreateLogger<Program>().LogError(ex, "Failed to apply migrations.");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
