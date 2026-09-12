using ExpenseTracker.Application.Common.Auth;
using ExpenseTracker.Application.Helpers;
using ExpenseTracker.Application.Interfaces.Common.Jwt;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ExpenseTracker.Application;

public static class AppDependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Jwt Authentication
        services.Configure<JwtOptions>(options =>
        {
            configuration.GetSection("JWT").Bind(options);
        });

        services.AddScoped<IJwtService, JwtService>();

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // AutoMapper
        services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

        return services;
    }
}
