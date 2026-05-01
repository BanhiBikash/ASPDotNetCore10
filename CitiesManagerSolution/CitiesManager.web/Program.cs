using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using CitiesManager.Infrastructure.DBcontext;
using Microsoft.AspNetCore.Mvc;
using CitiesManager.Core.Entities.IdentityUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ConsumesAttribute("application/json"));
}).AddXmlSerializerFormatters();

builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")));

//cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // your Vite dev server
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

//swagger
builder.Services.AddEndpointsApiExplorer(); //reading all the endpoints in the project and generate the swagger doc for them
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));

    //create documents for different versions of the API

    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Orders Version 1", Version = "1.0" });
    options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Orders Version 1", Version = "2.0" });
});       //generate the open api specification document (swagger doc) for the API, which can be used to describe the API's endpoints, request/response models, and other details in a standardized format.

//enable api versioning
builder.Services.AddApiVersioning(options =>
{
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
    //options.ApiVersionReader = new QueryStringApiVersionReader();
    //options.ApiVersionReader = new HeaderApiVersionReader("api-version");

    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; //v1
    options.SubstituteApiVersionInUrl = true;
}); ;

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 5;

}).AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders().AddUserStore<UserStore<ApplicationUser,ApplicationRole,ApplicationDBContext,Guid>>()
.AddRoleStore<RoleStore<ApplicationRole,ApplicationDBContext,Guid>>();

var app = builder.Build();

app.UseCors("AllowReactApp");

//var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "2.0");
});

// Configure the HTTP request pipeline.
app.UseHsts();
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
