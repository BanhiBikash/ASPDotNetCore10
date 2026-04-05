using CRUDExample.Filters.ActionFilters;
using CRUDExample.Filters.ResultFilters;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositries;
using RespositoryContract;
using Serilog;
using ServiceContracts;
using Services;

namespace CRUDExample
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection service, ConfigurationManager configuration)
        {
            //creating ilogger
            var logger = service.BuildServiceProvider().GetService<ILogger<ResponseHeaderFilter>>();

            //creating global filters Add(filter1,filter2)
            service.AddControllersWithViews(options => options.Filters.Add(new ResponseHeaderFilter(logger, "Global-Key", "Global-Value", 2)));

            service.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPropertiesAndHeaders | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            //Logging configuration
            //builder.Logging.ClearProviders().AddConsole().AddDebug();

            //add services into IoC container
            service.AddScoped<ICountriesRespository, CountriesRespository>();
            service.AddScoped<IPersonsRespository, PersonsRepository>();
            service.AddScoped<ICountriesService, CountriesService>();
            service.AddScoped<IPersonsService, PersonsService>();
            service.AddTransient<IndexResultFilter>();

            service.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            return service;
        }
    }
}
