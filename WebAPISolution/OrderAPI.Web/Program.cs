using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Infrastructure.DBContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddApiExplorer(options =>
{
    options.SubstituteApiVersionInUrl = true;
}); ;


//adding DB Context
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
});

builder.Services.AddEndpointsApiExplorer(); //reads all the action methods/endpoints

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() {Title="Orders Version 1", Version="1.0"});
    options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Orders Version 1", Version = "2.0" });
});     //create the open api code according to that

//swagger is told that how versions are substituted
builder.Services.AddApiVersioning().AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

///////
var app = builder.Build();

app.UseSwagger();   //creates endpoint for swagger.json
app.UseSwaggerUI(options =>{
    options.SwaggerEndpoint("/swagger/v1/swagger.json","1.0");
    options.SwaggerEndpoint("/swagger/v2/swagger.json","2.0");
});     //creates the swagger UI

// Configure the HTTP request pipeline.
app.UseHsts();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
