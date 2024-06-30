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
        services.AddScoped(typeof(IRepository<Event>), typeof(Repository<Event>));
        services.AddScoped(typeof(IRepository<Address>), typeof(Repository<Address>));
        services.AddScoped(typeof(IRepository<EventJob>), typeof(Repository<EventJob>));
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