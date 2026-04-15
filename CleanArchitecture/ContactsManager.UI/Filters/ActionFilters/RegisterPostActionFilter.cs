using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ContactsManager.Core.DTO;

namespace ContactsManager.UI.Filters.ActionFilters
{
    public class RegisterPostActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as Controller;

            //checking if the registerDTO is present in the action arguments
            if (!context.ActionArguments.ContainsKey("registerDTO"))
            {
                controller.ViewData["Errors"] = "User details not provided.";
                context.Result = controller.View("Register");
            }
            else          //checked if the registerDTO is null or any of its properties are null or empty
            {
                //Action Arguments will be of type object, so we need to cast it to RegisterDTO
                var registerDTO = context.ActionArguments["registerDTO"] as RegisterDTO;

                //use model state to validate the registerDTO
                if (!controller.ModelState.IsValid)
                {
                    controller.ViewData["Errors"] = controller.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    context.Result = controller.View("Register");
                }
            }

            await next();
        }
    }
}
