using CustomModelBinder.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers((options) =>
{
    options.ModelBinderProviders.Insert(0,new PersonBinderProvider());
});
var app = builder.Build();
app.MapControllers();
app.MapFallback(async (context) =>await  context.Response.WriteAsync("\nHello World! From Fallback"));

app.Run();
