using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters
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

        public void OnActionExecuted(ActionExecutedContext context)
        {
          
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{MethodName} method",nameof(ResponseHeaderFilter),nameof(OnActionExecuting));
            await next();
            _logger.LogInformation("{FilterName}.{MethodName} method", nameof(ResponseHeaderFilter), nameof(OnActionExecuted));
            context.HttpContext.Response.Headers[Key] = Value;
        }
    }
}
