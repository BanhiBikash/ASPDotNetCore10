using System;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

bool status  = false;


app.MapGet("/countries",async (context) =>
{
    if (!status)
    {
        status = true;
        context.Response.StatusCode = 200;
    }

    await context.Response.WriteAsync("\n 1, United States\n 2, Canada\n 3, United Kingdom\n 4, India\n 5, Japan");
});

app.MapGet("/countries/{id:int}",async (context) =>
{

    int id = int.Parse((string)context.Request.RouteValues["id"]!);

    if (id > 5)
    {
        if (id > 100)
        {
            if (!status)
            {
                context.Response.StatusCode = 400;
                status = true;
            }

            await context.Response.WriteAsync("\nThe CountryID should be between 1 and 100");
        }
        else
        {
            if (!status)
            {
                context.Response.StatusCode = 404;
                status = true;
            }

            await context.Response.WriteAsync("\n[No Country]");
        }
    }
    else
    {
        switch(id)
        {
            case (int)Countries.United_States:
                await context.Response.WriteAsync($"\nCountryID: {id}, CountryName: {Countries.United_States}");
                break;
            case (int)Countries.Canada:
                await context.Response.WriteAsync($"\nCountryID: {id}, CountryName: {Countries.Canada}");
                break;
            case (int)Countries.United_Kingdom:
                await context.Response.WriteAsync($"\nCountryID: {id}, CountryName: {Countries.United_Kingdom}");
                break;
            case (int)Countries.India:
                await context.Response.WriteAsync($"\nCountryID: {id}, CountryName: {Countries.India}");
                break;
            case (int)Countries.Japan:
                await context.Response.WriteAsync($"\nCountryID: {id}, CountryName: {Countries.Japan}");
                break;
        }

    }
});

app.MapFallback(async () =>
{
    await Task.FromResult("\nFallback route: The requested resource was not found."); 
});

app.Run();

public enum Countries
{
    United_States = 1,
    Canada,
    United_Kingdom,
    India,
    Japan
}
