﻿using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Helpers;
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
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IEventJobRepository, EventJobRepository>();
    }

    internal static void SetupServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<JwtBuilder>();
        services.AddScoped<IJwtClaimService, JwtClaimService>();
    }

    internal static void SetupContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<Context>(options =>
        {
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(Context).Assembly.FullName));
        });
    }
}