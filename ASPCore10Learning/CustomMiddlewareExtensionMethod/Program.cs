using CustomMiddlewareExtensionMethods.CustomMiddlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<Custom1>();
builder.Services.AddTransient<Custom2>();
builder.Services.AddTransient<Custom3>();

//building the app with all the dependencies
var app = builder.Build();

//using extension method 
app.UseCustom1();
app.UseCustom2();
app.UseCustom3();

app.Run();
