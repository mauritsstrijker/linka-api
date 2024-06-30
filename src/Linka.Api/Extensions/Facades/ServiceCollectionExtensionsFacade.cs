using Linka.Infrastructure.Extensions.Facades;
using Linka.Application.Extensions.Facades;

namespace Linka.Api.Extensions.Facades
{
    public static class ServiceCollectionExtensionsFacade
    {
        public static IServiceCollection Setup
        (
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.ConfigureInfrastructureApp(configuration);

            //services.SetupAuthentication(configuration);
            services.SetupControllers(configuration);

            services.SetupSwagger(configuration);
            services.ConfigureApplicationApp();

            services.SetupServices();
            return services;
        }
    }
}
