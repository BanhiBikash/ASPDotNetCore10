var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseStaticFiles();
app.MapControllers();
app.MapFallback(async (Context) => await Context.Response.WriteAsync("Hello World! From Fallback"));

app.Run();
