using ContactsManager.UI.Controllers;
using ContactsManager.UI.Filters.SkipFilters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.UI.Filters.ActionFilters
{
    public class ResponseHeaderFilter : IAsyncActionFilter,IOrderedFilter
    {

        private readonly ILogger<ResponseHeaderFilter> _logger;
        private readonly string Key;
        private readonly string Value;

        public int Order { get; set; }

        public ResponseHeaderFilter(ILogger<ResponseHeaderFilter> logger, string key, string value, int order)
        {
            _logger = logger;
            Key = key;
            Value= value;
            Order = order;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //if the filters list cobtain the skip repsonse filter
            if (context.Filters.OfType<SkipResponseFilter>().Any())
            {
                return;
            }

            _logger.LogInformation("{FilterName}.Before method",nameof(ResponseHeaderFilter));
            await next();
            _logger.LogInformation("{FilterName}.After method", nameof(ResponseHeaderFilter));
            context.HttpContext.Response.Headers[Key] = Value;
        }
    }
}
