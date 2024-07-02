namespace Linka.Api.Extensions.Facades
{
    public static class ApplicationBuilderExtensionsFacade
    {
        public static WebApplication Setup(this WebApplication app)
        {
            app.SetupSwagger();
            app.MigrateDatabase();
            app.SetupCors();
            app.SetupControllers();

            return app;
        }
    }
}
