using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Linka.Application.Extensions;
public static class ServiceCollectionExtensions
{
    //public static IServiceCollection SetupValidation(this IServiceCollection services)
    //{
    //    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    //    return services;
    //}

    public static IServiceCollection SetupMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }

    public static IServiceCollection SetupServices(this IServiceCollection services)
    {
       
        return services;
    }
}