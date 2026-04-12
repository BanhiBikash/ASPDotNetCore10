using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ServiceContracts;
using Services;
using StocksAppWithFilters.Models;

namespace StocksAppWithFilters.Filters.ActionFilters
{
    public class OrderActionFilter : IAsyncActionFilter
    {

        private readonly IStocksService _stocksService;

        public OrderActionFilter(IStocksService stockService)
        {
            _stocksService = stockService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var metadataProvider = context.HttpContext.RequestServices.GetRequiredService<IModelMetadataProvider>();

            OrdersList ordersList = new OrdersList()
            {
                BuyOrdersList = await _stocksService.GetBuyOrderList(),
                SellOrdersList = await _stocksService.GetSellOrderList()
            };

            if(ordersList.BuyOrdersList == null && ordersList.SellOrdersList == null)
            {
                context.Result = new ViewResult()
                {
                    ViewName = "Index",
                    ViewData = new ViewDataDictionary<StockData>(metadataProvider, context.ModelState)
                    {
                        Model = new StockData() { stockName = "Microsoft Technologies", stockSymbol = "MSFT", stockPrice = 327.6m , ErrorMessages = new List<string>() { "No Order Data" } }
                    }
                };
            }
            await next();
        }
    }
}
