using AuthService.Application.Interfaces.Repositories;
using AuthService.Application.Interfaces.Services;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.Security;
using AuthService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;


namespace AuthService.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core
        services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SQLServer")));
        var conn = configuration.GetConnectionString("SQLServer");
        Console.WriteLine(conn); // should print full connection string
        // Repositories & Services
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        // JWT Options + Token Service
        services.Configure<JwtOptions>(configuration.GetSection("Jwt")); // binds Jwt section
        services.AddScoped<ITokenService, JwtTokenService>();

        // Redis
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));
        services.AddScoped<IRedisTokenStore, RedisTokenStore>();

        return services;
    }
}
