using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Runtime.InteropServices;

namespace StocksAppWithFilters.Filters.ActionFilters
{
    public class IndexActionFilter : IActionFilter,IOrderedFilter
    {
        private readonly ILogger<IndexActionFilter> _logger;
        private readonly IConfiguration _configurtion;

        public IndexActionFilter(ILogger<IndexActionFilter> logger, IConfiguration configurtion)
        {
            _logger = logger;
            _configurtion = configurtion;
        }

        public int Order{ get; }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName} method", nameof(IndexActionFilter),nameof(OnActionExecuted));
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{FilterName}.{MethodName} method", nameof(IndexActionFilter), nameof(OnActionExecuting));
            
            if (string.IsNullOrEmpty(_configurtion.GetValue<string>("finnhubKey")))
            {
                context.Result = new ObjectResult("Fatal error: FinnhubKey is missing in configuration.")
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
