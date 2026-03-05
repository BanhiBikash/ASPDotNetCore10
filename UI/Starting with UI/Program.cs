using Services;
using ServicesContracts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPersonServices, PersonServices>();

var app = builder.Build();
app.UseStaticFiles();
app.MapControllers();

app.MapFallback(async (obj) => await obj.Response.WriteAsync( "Hello World! From Fallback"));

app.Run();
