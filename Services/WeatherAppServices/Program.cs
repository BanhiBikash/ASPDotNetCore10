using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICitiesService, CitiesService>();
builder.Services.AddScoped<ISpecificCity, SpecificCity>();

var app = builder.Build();

app.MapControllers();
app.UseStaticFiles();

app.MapFallback(async (con)=> {await con.Response.WriteAsync("Fallback."); });
app.Run();
