using ConfigAppSettings.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.Configure<Keys>(builder.Configuration.GetSection("MasterKey"));

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("MyKeys.json", optional: true, reloadOnChange: true);
});

var app = builder.Build();
app.UseStaticFiles();   
app.MapControllers();

string response = string.Empty;

app.Map(("/config"), async(HttpContext context) =>
{
    response = app.Configuration["MyKey"] + "\n";
    response += app.Configuration.GetValue<string>("MyKey") + "\n";
    response = response + app.Configuration.GetValue<int>("Digit", 7).ToString();
    await context.Response.WriteAsync(response);
});

app.Run();
