var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.MapFallback(async (context) => context.Response.WriteAsync("\nHello World!"));

app.Run();
