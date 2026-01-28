using System;
using System.Collections.Generic;
using System.Text;
using AuthService.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, Services.AuthService>();
            return services;
        }
    }
}
