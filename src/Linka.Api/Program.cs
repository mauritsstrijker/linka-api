using Linka.Api.Extensions.Facades;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Setup(builder.Configuration);

var app = builder.Build();
app.Setup();

app.Run();