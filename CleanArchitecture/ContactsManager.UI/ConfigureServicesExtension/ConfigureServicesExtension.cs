using ContactsManager.UI.Filters.ActionFilters;
using ContactsManager.UI.Filters.ResultFilters;
using ContactsManager.Core.Services;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using ContactsManager.Domain.RepositoryContracts;
using ContactsManager.Infrastructure.DBContext;

namespace ContactsManager.UI.ConfigureServicesExtension
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
            service.AddScoped<IPersonsSortService, PersonsSortService>();
            service.AddScoped<IPersonsDeleteService, PersonsDeleteService>();
            service.AddScoped<IPersonsAddService, PersonsAddService>();
            service.AddScoped<IPersonsGetService, PersonsGetService>();
            service.AddScoped<IPersonsUpdateService, PersonsUpdateService>();
            service.AddTransient<IndexResultFilter>();

            service.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            return service;
        }
    }
}
