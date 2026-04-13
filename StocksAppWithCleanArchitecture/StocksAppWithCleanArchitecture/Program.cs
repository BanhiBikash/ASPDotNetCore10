using Microsoft.EntityFrameworkCore;
using StocksApp.Infrastructure.Repository;
using StocksApp.Core.RespositoryContract;
using StocksApp.Infrastructure.DBContext;
using StocksApp.Core.ServiceContracts;
using StocksApp.Core.Services;
using Rotativa;
using StocksApp.UI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StocksDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:default"));
});

//services
builder.Services.AddHttpClient();
builder.Services.AddScoped<IStocksService, StocksService>();
builder.Services.AddScoped<IStocksRepository, StocksRepository>();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseCustomExceptionMiddleware();
    app.UseExceptionHandler("/Error");
}

    app.MapControllers();
app.UseStaticFiles();

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot",wkhtmltopdfRelativePath:"Rotativa");
app.MapFallback(async (context) => context.Response.WriteAsync("Fallback!"));

app.Run();
