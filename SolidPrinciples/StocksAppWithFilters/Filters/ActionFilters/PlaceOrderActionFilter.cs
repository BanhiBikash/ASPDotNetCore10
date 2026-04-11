using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using StocksAppWithFilters.Models;

namespace StocksAppWithFilters.Filters.ActionFilters
{
    public class PlaceOrderActionFilter : IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //taking the stockData from the action arguments
            StockData? stockData = context.ActionArguments["stockData"] as StockData;

            //when stockData is null
            if (stockData == null)
            {
                var metadataProvider = context.HttpContext.RequestServices.GetRequiredService<IModelMetadataProvider>();

                StockData errorData = new StockData()
                {
                    ErrorMessages = new List<string>() { "Invalid stock data. Please provide valid stock information." }
                };

                context.Result = new ViewResult()
                {
                    ViewName = "Index",
                    ViewData = new ViewDataDictionary<StockData>(metadataProvider, context.ModelState)
                    {
                        Model = errorData
                    }
                };
            }

            //checking if any of the required properties of stockData is null or invalid
            foreach (var property in typeof(StockData).GetProperties())
            {
                if (property.GetValue(stockData) == null && (nameof(property) == nameof(stockData.stockName) || nameof(property) == nameof(stockData.stockSymbol) || nameof(property) == nameof(stockData.stockPrice) || nameof(property) == nameof(stockData.orderQuantity) || nameof(property) == nameof(stockData.orderAction) ))
                {
                    var metadataProvider = context.HttpContext.RequestServices.GetRequiredService<IModelMetadataProvider>();

                    StockData errorData = new StockData()
                    {
                        ErrorMessages = new List<string>() { $"Invalid stock data. Please provide valid stock information, {nameof(property)} is null or invalid." }
                    };

                    context.Result = new ViewResult()
                    {
                        ViewName = "Index",
                        ViewData = new ViewDataDictionary<StockData>(metadataProvider, context.ModelState)
                        {
                            Model = errorData
                        }
                    };
                }
            }

            await next();
        }
    }
}
