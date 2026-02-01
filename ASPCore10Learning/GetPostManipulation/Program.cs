using Microsoft.Extensions.Primitives;
using System.IO;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext obj) =>
{
    //this body will read the post request body
    StreamReader reader = new StreamReader(obj.Request.Body);
    string body= await reader.ReadToEndAsync();

    //we parse the string of queries into dictionry using query helpers
    Dictionary<string, StringValues> postData = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);

    await obj.Response.WriteAsync($"\nPost Request Body Data Received:");

    try
    {

        foreach (var key in postData.Keys)
        {
            await obj.Response.WriteAsync($"\nKey: {key}, Value: {postData[key]}");
        }

    }
    catch(Exception ex)
    {
        await obj.Response.WriteAsync($"\nException Occurred: {ex.Message}");
    }
});

app.Run();
