using StocksApp.Services;
using StocksApp.ServiceContracts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<FinnhubService>();

var app = builder.Build();
app.UseStaticFiles();
app.MapControllers();
app.MapFallback(async (ctx) => await ctx.Response.WriteAsync( "Hello World! From Fallback"));

app.Run();
