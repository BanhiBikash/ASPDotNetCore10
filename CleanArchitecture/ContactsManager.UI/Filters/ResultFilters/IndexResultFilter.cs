using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.UI.Filters.ResultFilters
{
    public class IndexResultFilterAttribute : Attribute, IFilterFactory
    {
        private readonly string? _key;
        private readonly string? _value;


        public IndexResultFilterAttribute(string? key, string? value) 
        { 
           _key = key;
           _value = value;
        }

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new IndexResultFilter(serviceProvider.GetService<ILogger<IndexResultFilter>>()) {Key=_key, Value = _value };
        }
    }

    public class IndexResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<IndexResultFilter> _logger;

        public string Key { get; set; }
        public string? Value { get; set; }
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
