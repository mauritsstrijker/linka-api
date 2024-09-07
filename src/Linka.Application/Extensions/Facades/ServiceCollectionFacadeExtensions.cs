using Linka.Application.Shared.Behavior;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Linka.Application.Extensions.Facades;
public static class ServiceCollectionFacadeExtensions
{
    public static IServiceCollection ConfigureApplicationApp(this IServiceCollection services)
    {
        services.SetupValidation();
        services.SetupMediatR();
        services.SetupServices();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}