using ContactsManager.UI.Middlewares;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ContactsManager.UI.ConfigureServicesExtension;
using Serilog.Sinks.SystemConsole;
using Serilog.Sinks.File;
using Serilog.Sinks.Seq;
using Serilog.Sinks.MSSqlServer;
using Serilog.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False


//services
builder.Services.ConfigureServices(builder.Configuration);
//services

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(service);
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
 app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandlingCustomMiddleware();
    app.UseExceptionHandler("/Error");
}

//use https redirection
app.UseHsts();
app.UseHttpsRedirection();

//logger
app.Logger.LogDebug("debug-message");
app.Logger.LogInformation("information-message");
app.Logger.LogWarning("warning-message");
app.Logger.LogError("error-message");
app.Logger.LogCritical("critical-message");

//rotavita for pdf generation
Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

//middlewares
app.UseHttpLogging();
app.UseStaticFiles(); 
app.UseRouting();       //Matches the incoming request to the route template and selects the appropriate controller and action method
app.UseAuthentication();    //Reads Identity cookie
app.UseAuthorization();    //Checks if the user is authorized to access the resource or not based on the policies and roles defined in the application
app.MapControllers();   //executes the filter pipeline and the selected action method and generates a response
app.Run();
