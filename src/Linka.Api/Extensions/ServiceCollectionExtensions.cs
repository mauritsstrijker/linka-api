using Linka.Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Linka.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public const string AuthorizationSecretPath = "Authentication:Secret";

        public static IServiceCollection SetupServices(this IServiceCollection services)
        {

            return services;
        }

        public static IServiceCollection SetupSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.MapType<FileResult>(() => new OpenApiSchema { Type = "file" });
                options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = configuration.GetValue<string>("swagger:title"),
                    Version = configuration.GetValue<string>("swagger:version"),
                });
            });

            return services;
        }

        public static IServiceCollection SetupControllers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddCors(p => p.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            return services;
        }

        public static IServiceCollection SetupAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[AuthorizationSecretPath]))
                };

            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Volunteer", policy =>
                    policy.RequireClaim("type", "volunteer"));

                options.AddPolicy("Organization", policy =>
                    policy.RequireClaim("type", "organization"));
            });

            return services;
        }
    }
}
