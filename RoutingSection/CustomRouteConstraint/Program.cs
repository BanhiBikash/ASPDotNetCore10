using CustomRouteConstraint.CustomConstraints;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

//option is route options class instance
builder.Services.AddRouting((options) =>
{
    options.ConstraintMap.Add("months",typeof(CustomMonthConstraint));
});

var app = builder.Build();

app.Map("registration-month/{year:int:range(1900,2025)}-{month:months}", async (context) =>
{
    await context.Response.WriteAsync($"\nValid Month Entered is {context.Request.RouteValues["month"]}-{context.Request.RouteValues["year"]}");
});

//fallback route
app.MapFallback(async (context) =>
{
    await context.Response.WriteAsync("Invalid Month. Please enter a valid month (Jan, Apr, Aug, Dec)");
});

app.Run();
