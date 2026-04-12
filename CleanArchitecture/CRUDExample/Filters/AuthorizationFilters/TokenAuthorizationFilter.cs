using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection.Metadata.Ecma335;

namespace CRUDExample.Filters.AuthorizationFilters
{
    public class TokenAuthorizationfilter : IAsyncAuthorizationFilter

    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Cookies.ContainsKey("Auth-Key"))
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
            else if (context.HttpContext.Request.Cookies["Auth-Key"]!="A100")
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
            else
            {
                
            }
        }
    }
}
