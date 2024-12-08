using Linka.Api.Extensions.Facades;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Setup(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (error != null)
        {
            var response = new
            {
                success = false,
                error = new
                {
                    message = error.Message,
                }
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    });
});

app.Setup();

app.Run();