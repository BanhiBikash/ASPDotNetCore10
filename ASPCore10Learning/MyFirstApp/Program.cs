var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World! This is my first ASP Dot Net Core 10 App.");

app.Run(async (HttpContext obj) =>
{
    obj.Response.Headers["Content-type"] = "text/html";

    //if it is a get type request
    if (obj.Request.Method.Equals("GET"))
    {
        //if it contains id query
        if (obj.Request.Query.ContainsKey("id"))
        {
            if (obj.Request.Query["id"].Equals("1"))
            {
                obj.Response.StatusCode = 200;
                await obj.Response.WriteAsync($"\nThe query ID is {obj.Request.Query["id"]}");
            }
            else
            {
                obj.Response.StatusCode = 300;
                await obj.Response.WriteAsync($"\nThe query ID is {obj.Request.Query["id"]},not in record.");
            }
        }
        else
        {
            obj.Response.StatusCode = 400;
            await obj.Response.WriteAsync($"\nThe query is not received.");
        }

    }
    

    //await obj.Response.WriteAsync($"This is the start of the program.");
    //obj.Response.Headers.Add("Custom-Header","CustomHeaderValue2");
    //obj.Response.StatusCode = 302;
    //await obj.Response.WriteAsync($"This is a custom body response. Custom Response Header:- {obj.Response.Headers["Custom-Header"]}");
    //await obj.Response.WriteAsync($"This is the end of the program.");
});

app.Run();
