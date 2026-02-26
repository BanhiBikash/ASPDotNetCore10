using StockAppFinal.Services;
using StockAppFinal.ServiceContracts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IFinnhubService,FinnhubService>();

var app = builder.Build();
app.UseStaticFiles();
app.MapControllers();

app.MapFallback(async (context) => { await context.Response.WriteAsync("This is Fallback"); });

app.Run();
