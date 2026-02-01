
using Microsoft.Extensions.Primitives;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext obj) =>
{
    // Handle POST request
    if (obj.Request.Method == "GET")
    { 
        bool entries = true;
        bool setCode = false;

        if (!obj.Request.Query.ContainsKey("firstNumber"))
        {
            if (!setCode)
            {
                obj.Response.StatusCode = 400;
                setCode = true;
            }  

            await obj.Response.WriteAsync("\nInvalid input for 'firstNumber'");
            entries = false;
        }

        if (!obj.Request.Query.ContainsKey("secondNumber"))
        {
            if (!setCode)
            {
                obj.Response.StatusCode = 400;
                setCode = true;
            }
            await obj.Response.WriteAsync("\nInvalid input for 'secondNumber'");
            entries = false;
        }

        if (!obj.Request.Query.ContainsKey("operation"))
        {
            if (!setCode)
            {
                obj.Response.StatusCode = 400;
                setCode = true;
            }
            await obj.Response.WriteAsync("\nInvalid input for 'operation'");
            entries = false;
        }

        //all entries are found
        if (entries) 
        {

            //extracting int values from the queries
            int.TryParse(obj.Request.Query["firstNumber"],out int x);
            int.TryParse(obj.Request.Query["secondNumber"], out int y);

            //addition
            if (obj.Request.Query["operation"] == "add")
            {
                if (!setCode)
                {
                    obj.Response.StatusCode = 200;
                    setCode = true;
                }
                await obj.Response.WriteAsync("\n"+(x + y).ToString());
            }
            else if (obj.Request.Query["operation"] == "subtract")
            { 
                if (!setCode)
                {
                    obj.Response.StatusCode = 200;
                    setCode = true;
                }
            await obj.Response.WriteAsync("\n" + (x - y).ToString());

            }
            else if (obj.Request.Query["operation"] == "multiply")
            {
                if (!setCode)
                {
                    obj.Response.StatusCode = 200;
                    setCode = true;
                }
                await obj.Response.WriteAsync("\n" + (x * y).ToString());

            }
            else if (obj.Request.Query["operation"] == "divide")
            {
                if (!setCode)
                {
                    obj.Response.StatusCode = 200;
                    setCode = true;
                }
                await obj.Response.WriteAsync("\n" + (x / y).ToString());

            }
            else if (obj.Request.Query["operation"] == "remainder")
            {
                if (!setCode)
                {
                    obj.Response.StatusCode = 200;
                    setCode = true;
                }
                await obj.Response.WriteAsync("\n" + (x % y).ToString());

            }
            else
            {
                if (!setCode)
                {
                    obj.Response.StatusCode = 400;
                    setCode = true;
                }
                await obj.Response.WriteAsync("\nInvalid input for 'operation'.");
            }
        }
    }
});

app.Run();
