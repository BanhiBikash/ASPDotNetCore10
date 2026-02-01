var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//guid is a unique Hexadecimal value which is commonly used for Ids since it is unique across systems

//default parameter constraints
app.Map("report-date/{date:datetime=01/01/2000}",
    async (context) =>
    {
      await  context.Response.WriteAsync($"Report Date: {context.Request.RouteValues["date"]}");
    });

//optional parameter constraints
app.Map("emp/{empid:guid?}", async (context) => {
    if (context.Request.RouteValues.ContainsKey("empid"))
    {
        Guid? id = Guid.Parse(Convert.ToString(context.Request.RouteValues["empid"]));
        await context.Response.WriteAsync($"\nThe employee id is {id}.");
    }
    else
    {
        await context.Response.WriteAsync("\n No employee id provided");
    }
});

app.Map("sales-report/{year:int:range(1900,2025)}-{month:regex(^(jan|apr|aug|dec)$)}", async (context) =>
{
    var year = context.Request.RouteValues["year"];
    var month = context.Request.RouteValues["month"];
    await context.Response.WriteAsync($"Sales report for {month}-{year}");
});

//we can put multiple constraints on a single parameter by separating them with a colon

//fallback middleware or terminal endpoint
app.MapFallback(async context =>
{
    await context.Response.WriteAsync("No matching route found.");
});

app.Run();
