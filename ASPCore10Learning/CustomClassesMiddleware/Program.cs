using CustomClassesMiddleware.CustomMiddlewares;

var builder = WebApplication.CreateBuilder(args);

//importing custom middleware classes
builder.Services.AddTransient<Custom1>();
builder.Services.AddTransient<Custom2>();
builder.Services.AddTransient<Custom3>();

//building the app
var app = builder.Build();

//going through all middlewares
app.UseMiddleware<Custom1>();
app.UseMiddleware<Custom2>();
app.UseMiddleware<Custom3>();

app.Run();
