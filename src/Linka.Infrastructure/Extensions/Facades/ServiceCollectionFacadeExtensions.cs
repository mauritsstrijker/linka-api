using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Linka.Infrastructure.Extensions.Facades;
public static class ServiceCollectionFacadeExtensions
{
    public static IServiceCollection ConfigureInfrastructureApp
        (
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        services.SetupRepositories();
        services.SetupServices();
        services.SetupContext(configuration["ConnectionStrings:DbContext"]);
        return services;
    }
}