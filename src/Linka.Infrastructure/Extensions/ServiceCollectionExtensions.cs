using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Helpers;
using Linka.Application.Repositories;
using Linka.Infrastructure.Data;
using Linka.Infrastructure.Data.Repositories;
using Linka.Infrastructure.Services;
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
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IFollowRepository, FollowRepository>();
        services.AddScoped<IPostCommentRepository, PostCommentRepository>();
        services.AddScoped<IConnectionRequestRepository, ConnectionRequestRepository>();
        services.AddScoped<IConnectionRepository, ConnectionRepository>();
        services.AddScoped<IJobVolunteerActivityRepository, JobVolunteerActivityRepository>();
        services.AddScoped<IFeedService, FeedService>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductReservationRepository, ProductReservationRepository>();
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