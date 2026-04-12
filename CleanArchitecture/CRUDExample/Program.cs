using CRUDExample;
using CRUDExample.Filters.ActionFilters;
using CRUDExample.Filters.ResultFilters;
using CRUDExample.Middlewares;
using Entities;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Utils;
using Repositories;
using Repositries;
using RespositoryContract;
using Serilog;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);

//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False


//services
builder.Services.ConfigureServices(builder.Configuration);
//services

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(service);
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
 app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandlingCustomMiddleware();
    app.UseExceptionHandler("/Error");
}

    app.Logger.LogDebug("debug-message");
app.Logger.LogInformation("information-message");
app.Logger.LogWarning("warning-message");
app.Logger.LogError("error-message");
app.Logger.LogCritical("critical-message");

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

app.UseStaticFiles();
app.MapControllers();
app.UseHttpLogging();

app.Run();
