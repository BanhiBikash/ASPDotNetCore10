using Asp.Versioning;
using CitiesManager.web.DBcontext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Asp.Versioning.ApiExplorer;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ConsumesAttribute("application/json"));
}).AddXmlSerializerFormatters();

builder.Services.AddDbContext<ApplicationDBContext>(options=>options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")));

//swagger
builder.Services.AddEndpointsApiExplorer(); //reading all the endpoints in the project and generate the swagger doc for them
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));

    //create documents for different versions of the API
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Cities Web API",
        Version = "1.0"
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Cities Web API",
        Version = "2.0"
    });
});       //generate the open api specification document (swagger doc) for the API, which can be used to describe the API's endpoints, request/response models, and other details in a standardized format.

builder.Services.AddApiVersioning()
 .AddApiExplorer(options =>
 {
     options.GroupNameFormat = "'v'VVV"; //v1
     options.SubstituteApiVersionInUrl = true;
 });

//enable api versioning
builder.Services.AddApiVersioning(options =>
{
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
    //options.ApiVersionReader = new QueryStringApiVersionReader();
    //options.ApiVersionReader = new HeaderApiVersionReader("api-version");

    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});


var app = builder.Build();

app.UseSwagger();   //create endpoints for swagger.json
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Cities Web API v1.0");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "Cities Web API v2.0");
});         //creates swagger UI for testing all Web API endpoints, which provides an interactive interface for developers to explore and test the API's functionality directly from the browser. It allows users to view the API documentation, send requests to the endpoints, and see the responses in a user-friendly format, making it easier to understand and interact with the API.

// Configure the HTTP request pipeline.
app.UseHsts();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
