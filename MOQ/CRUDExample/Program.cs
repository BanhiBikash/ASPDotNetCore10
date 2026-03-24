using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositries;
using RespositoryContract;
using ServiceContracts;
using Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPropertiesAndHeaders | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
});

//Logging configuration
//builder.Logging.ClearProviders().AddConsole().AddDebug();

builder.Host.UseSerilog((HostBuilderContext context,IServiceProvider service,LoggerConfiguration configuration) => 
{
    configuration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(service);
});

//add services into IoC container
builder.Services.AddScoped<ICountriesRespository, CountriesRespository>();
builder.Services.AddScoped<IPersonsRespository, PersonsRepository>();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
 app.UseDeveloperExceptionPage();
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
