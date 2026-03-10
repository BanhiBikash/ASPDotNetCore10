using Services;
using ServicesContracts;
using Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IPersonServices, PersonServices>();
builder.Services.AddDbContext<PersonsDBContext>((options) =>
{
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
});

var app = builder.Build();
app.UseStaticFiles();
app.MapControllers();

app.MapFallback(async (obj) => await obj.Response.WriteAsync("Hello World! From Fallback"));

app.Run();