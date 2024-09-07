using Linka.Application.Common;
using Linka.Domain.Entities;
using Linka.Infrastructure.Data;
using Linka.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Linka.Infrastructure.Extensions;
internal static class ServiceCollectionExtensions
{
    internal static void SetupRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
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