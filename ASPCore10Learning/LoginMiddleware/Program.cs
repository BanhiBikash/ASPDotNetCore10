using LoginMiddleware.CustomMiddlewares;
using Microsoft.Extensions.Primitives;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<Verify>();

var app = builder.Build();

app.UseMid();
app.UseVerify();

app.Run(async (obj) =>
{
    await obj.Response.WriteAsync("Login Successful");
});

app.Run();
