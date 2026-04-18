using ContactsManager.UI.Filters.ActionFilters;
using ContactsManager.UI.Filters.ResultFilters;
using ContactsManager.Core.Services;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using ContactsManager.Domain.RepositoryContracts;
using ContactsManager.Infrastructure.DBContext;
using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.ConfigureServicesExtension
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection service, ConfigurationManager configuration)
        {
            //creating ilogger
            var logger = service.BuildServiceProvider().GetService<ILogger<ResponseHeaderFilter>>();

            //creating global filters Add(filter1,filter2)
            service.AddControllersWithViews(options => { options.Filters.Add(new ResponseHeaderFilter(logger, "Global-Key", "Global-Value", 2));
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

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


            //adding database context
            service.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            //use identity
            service.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 3;
            }).
                AddDefaultTokenProviders(). //token for otps etc
                AddEntityFrameworkStores<ApplicationDbContext>().   //setting the db context/telling the database name
                AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>().    //cause db can't be accessed directly so repository for UserTable
                AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>(); //cause db can't be accessed directly so repository for RoleTable

            //authorize
            service.AddAuthorization(options =>
            {       //requires authorization for all controllers and actions by default, unless [AllowAnonymous] is used
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                options.AddPolicy("NotAuthenticated", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        return !context.User.Identity.IsAuthenticated;
                    });
                });
            });

            //fallback policy for authorization, if no [AllowAnonymous] is used then it will require authentication by default
            service.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            });


            return service;
        }
    }
}
