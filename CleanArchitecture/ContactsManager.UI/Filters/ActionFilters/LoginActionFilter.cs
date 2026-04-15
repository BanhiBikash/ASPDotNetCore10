using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.UI.Filters.ActionFilters
{
    public class LoginActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<LoginActionFilter> _logger;

        public LoginActionFilter(ILogger<LoginActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("Prexecution of {0} method.",nameof(LoginActionFilter));
            var controller = context.Controller as Controller;

            if(!context.ActionArguments.ContainsKey("loginDTO"))
            {
                controller.ViewBag.Errors = new List<string>() { "UserName or Password not supplied" };
                context.Result = controller.View("Login");
                return;
            }
            await next();
        }
    }
}
