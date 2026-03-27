using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters
{
    public class ResponseHeaderFilter : IActionFilter
    {

        private readonly ILogger<ResponseHeaderFilter> _logger;
        private readonly string Key;
        private readonly string Value;

        public ResponseHeaderFilter(ILogger<ResponseHeaderFilter> logger, string key, string value)
        {
            _logger = logger;
            Key = key;
            Value= value;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName} method", nameof(ResponseHeaderFilter), nameof(OnActionExecuted));
            context.HttpContext.Response.Headers[Key] = Value;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName} method",nameof(ResponseHeaderFilter),nameof(OnActionExecuting));
        }
    }
}
