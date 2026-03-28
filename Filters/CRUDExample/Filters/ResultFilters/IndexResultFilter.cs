using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResultFilters
{
    public class IndexResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<IndexResultFilter> _logger;

        public IndexResultFilter(ILogger<IndexResultFilter> logger) 
        {
            _logger = logger;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{MethodName}-before",nameof(IndexResultFilter),nameof(OnResultExecutionAsync));
            context.HttpContext.Response.Headers["Last-modified"] = DateTime.Now.ToString("f");
            await next();
            _logger.LogInformation("{FilterName}.{MethodName}-after",nameof(IndexResultFilter),nameof(OnResultExecutionAsync));
        }
    }
}
