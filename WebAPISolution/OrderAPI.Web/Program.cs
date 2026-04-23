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

});     //create the open api code according to that
var app = builder.Build();

app.UseSwagger();   //creates endpoint for swagger.json
app.UseSwaggerUI();     //creates the swagger UI

// Configure the HTTP request pipeline.
app.UseHsts();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
