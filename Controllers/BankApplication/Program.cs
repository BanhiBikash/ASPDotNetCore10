var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
//app.MapGet("/",async (Context) => Context.Response.WriteAsync("\nWelcome to the Best Bank."));

app.MapFallback(async (Context) => Context.Response.WriteAsync("\nInvalid request."));
app.Run();
