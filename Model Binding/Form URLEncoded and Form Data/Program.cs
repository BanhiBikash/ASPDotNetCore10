var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();

app.MapFallback(async () => "\nInvalid input");

app.Run();
