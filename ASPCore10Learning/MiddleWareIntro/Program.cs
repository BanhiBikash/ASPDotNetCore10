using System.Threading.Tasks.Dataflow;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (HttpContext obj, RequestDelegate next) => 
{
   await obj.Response.WriteAsync("\nHello from first middleware 1!\n");
    await obj.Response.WriteAsync("\nGoing to middleware 2!\n");
    await next(obj);
    await obj.Response.WriteAsync("\nBack to middleware 1 after middleware 2!\n");
});

app.Use(async (obj,next) => {
    await obj.Response.WriteAsync("\nHello from first middleware 2!\n");
    await obj.Response.WriteAsync("\nGoing to middleware 3-The terminal/ShortCircuiting middleware!\n");
    await next(obj);
    await obj.Response.WriteAsync("\nBack to middleware 2 after terminal middleware!\n");
});
app.Run(async (obj) =>
{
    await obj.Response.WriteAsync("\nHello from terminal middleware 3!\n");
    await obj.Response.WriteAsync("\nThis is the end of the pipeline!\n");
});

app.Run();