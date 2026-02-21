var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.MapControllers();
app.UseStaticFiles();
app.MapFallback(async (con)=> {await con.Response.WriteAsync("Fallback."); });
app.Run();
