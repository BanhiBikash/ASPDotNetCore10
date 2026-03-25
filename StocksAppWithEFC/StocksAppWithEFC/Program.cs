using Entities.DBContext;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties;
});

//setting up serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration configuration) => 
{ configuration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services); });

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddDbContext<StocksDBContext>((options) => { options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")); });
builder.Services.AddHttpClient();
var app = builder.Build();
app.UseStaticFiles();
app.MapControllers();
app.UseHttpLogging();
app.MapFallback(async (context) =>await context.Response.WriteAsync( "Hello World! From Fallback"));
Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

app.Run();
