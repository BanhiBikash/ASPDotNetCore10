var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//this handles both GET and POST requests at map1 path
app.Map("/map1", async (obj) => { await obj.Response.WriteAsync($"\nall requests from url {obj.Request.Path}"); });

//this handles both GET and POST requests at map2 path
app.Map("/map2",async (obj) => { await obj.Response.WriteAsync($"\nall requests from url {obj.Request.Path}"); });

//this handles GET requests at map1 path
app.MapGet("/map1", async (obj) => { await obj.Response.WriteAsync($"\nGet requests from url {obj.Request.Path}"); });

//this handles GET POST requests at map2 path
app.MapGet("/map2", async (obj) => { await obj.Response.WriteAsync($"\nGet requests from url {obj.Request.Path}"); });

//this handles POST requests at map1 path
app.MapPost("/map1", async (obj) => { await obj.Response.WriteAsync($"\nPost requests from url {obj.Request.Path}"); });

//this handles POST requests at map2 path
app.MapPost("/map2", async (obj) => { await obj.Response.WriteAsync($"\nPost requests from url {obj.Request.Path}"); });

//if no match found, this will be executed
app.MapFallback(async (obj) => { await obj.Response.WriteAsync($"\nNo match found for url {obj.Request.Path}"); });

//even if we have multiple matches, the most specific one will be executed

//the use method executes for all requests
app.Use(async (context, next) =>
{
    await context.Response.WriteAsync($"\nUse method executed for url {context.Request.Path} and executes first.");
    await next();
});

app.Run();
