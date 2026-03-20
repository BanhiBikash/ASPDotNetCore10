using Entities.DBContext;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddDbContext<StocksDBContext>((options) => { options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")); });
builder.Services.AddHttpClient();
var app = builder.Build();
app.UseStaticFiles();
app.MapControllers();
app.MapFallback(async (context) =>await context.Response.WriteAsync( "Hello World! From Fallback"));

app.Run();
