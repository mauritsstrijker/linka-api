namespace Linka.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication SetupControllers(this WebApplication app)
        {
            app.MapControllers();
            return app;
        }

        public static WebApplication SetupSwagger(this WebApplication app)
        {
            if (!app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }

        public static WebApplication SetupCors(this WebApplication app)
        {
            app.UseCors("corspolicy");

            return app;
        }

        public static WebApplication SetupAuthorization(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }

        public static WebApplication SetupMiddlewares(this WebApplication app)
        {

            return app;
        }
    }
}
