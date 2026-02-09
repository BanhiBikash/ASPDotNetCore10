var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();
app.MapFallback(async (context) =>await context.Response.WriteAsync("\nIn Fallback"));

app.Run();
