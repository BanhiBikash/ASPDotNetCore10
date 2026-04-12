using ContactsManager.UI.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ContactsManager.Core.DTO;
using System.Reflection.PortableExecutable;

namespace ContactsManager.UI.Filters.ActionFilters
{
    public class IndexActionFilter : IActionFilter
    {

        private readonly ILogger<IndexActionFilter> _logger;

        public IndexActionFilter(ILogger<IndexActionFilter> logger)
        {
            _logger = logger;
        }

        //Action executed
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName} method", nameof(IndexActionFilter), nameof(OnActionExecuted));

            //type casting the controller in persons type controller
            PersonsController personsController = (PersonsController)context.Controller;
            //string the data from the items to a dictionary type for later use
            Dictionary<string, object?>? Arguments = (Dictionary<string, object?>?)context.HttpContext.Items["Arguments"];

            //checking if the searchBy paramter is present
            if (Arguments.ContainsKey("searchBy"))
            {
                //storing the data in ViewBag/ViewData
                personsController.ViewBag.currentSearchBy = Convert.ToString(Arguments["searchBy"]);
            }

            //checking if the searchstring paramter is present
            if (Arguments.ContainsKey("searchString"))
            {
                //storing the data in ViewBag/ViewData
                personsController.ViewBag.currentSearchString = Convert.ToString(Arguments["searchString"]); ;
            }

            //checking if the sortBy paramter is present
            if (Arguments.ContainsKey("sortBy"))
            {
                //storing the data in ViewBag/ViewData
                personsController.ViewBag.currentSortBy = Convert.ToString(Arguments["sortBy"]);
            }

            //checking if the sortOrder paramter is present
            if (Arguments.ContainsKey("sortOrder"))
            {
                //storing the data in ViewBag/ViewData
                personsController.ViewBag.currentSortOrder = Convert.ToString(Arguments["sortOrder"]);
            }
            personsController.ViewBag.SearchFields = new Dictionary<string, string>()
               {{ 
                    nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.CountryID), "Country" },
                { nameof(PersonResponse.Address), "Address" }
            };
        }

        //Action Executing
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName} method",nameof(IndexActionFilter),nameof(OnActionExecuting));

            context.HttpContext.Items["Arguments"] = context.ActionArguments;

            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy =  Convert.ToString(context.ActionArguments["searchBy"]);

                List<string> searchByValues = new List<string>() {
                nameof(PersonResponse.PersonName),
                nameof (PersonResponse.Gender),
                nameof(PersonResponse.Address),
                nameof(PersonResponse.DateOfBirth),
                nameof(PersonResponse.Email),
                nameof(PersonResponse.Country)
                };

                if (!searchByValues.Contains(searchBy))
                {
                    context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                }
            }

            if (context.ActionArguments.ContainsKey("searchString"))
            {
                string? searchString = Convert.ToString(context.ActionArguments["searchString"]);
            }
        }
    }
}
