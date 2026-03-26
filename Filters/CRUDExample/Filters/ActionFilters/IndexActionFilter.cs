using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters
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
            _logger.LogInformation("Action executed Index Filter");
        }

        //Action Executing
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Action executing Index Filter");
        }
    }
}
