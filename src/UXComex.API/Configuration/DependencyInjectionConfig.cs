using FluentValidation.AspNetCore;
using UXComex.Application.Interfaces.Services;
using UXComex.Application.Services;
using UXComex.Domain.Repositories;
using UXComex.Domain.Validations;
using UXComex.Infra.Connection;
using UXComex.Infra.Repositories;

namespace UXComex.API.Configuration;

public static class DependencyInjectionConfig
{ 
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();

        //Services
        services.AddScoped<IProductAppService, ProductAppService>();
        services.AddScoped<IClientAppService, ClientAppService>();
        services.AddScoped<IOrderAppService, OrderAppService>();

        // Database
        services.AddScoped<ISqlDbConnection, SqlDbConnection>();

        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProductEntityValidator>());

        return services;
    }
}