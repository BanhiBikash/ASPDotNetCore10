using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositries;
using RespositoryContract;
using ServiceContracts;
using Services;
using Rotativa;
using StocksAppWithFilters.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StocksDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:default"));
});

//services
builder.Services.AddHttpClient();
builder.Services.AddScoped<IStocksService,StocksService>();
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
