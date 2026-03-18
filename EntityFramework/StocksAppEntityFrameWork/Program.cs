using Microsoft.EntityFrameworkCore;
using Entities;
using ServicesContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IStockService,StockService>();
builder.Services.AddDbContext<StocksDBContext>((options) =>
{
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
});
var app = builder.Build();
app.MapControllers();
app.MapFallback(async (context) =>await context.Response.WriteAsync( "Hello World! From Fallback"));
app.UseStaticFiles();
app.Run();
