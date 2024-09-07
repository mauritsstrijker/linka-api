using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Infrastructure.Data;
using Linka.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Linka.Infrastructure.Extensions;
internal static class ServiceCollectionExtensions
{
    internal static void SetupRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
    }

    internal static void SetupServices(this IServiceCollection services)
    {
    }

    internal static void SetupContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<Context>(options =>
        {
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(Context).Assembly.FullName));
        });
    }
}