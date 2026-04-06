using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace StocksAppWithFilters.ViewComponents
{
    public class StockBoxViewComponent: ViewComponent
    {
        private readonly IStocksService _stockService;
        private readonly IConfiguration _configuration;

        public StockBoxViewComponent(IStocksService stocksService, IConfiguration configuration)
        {
            _stockService = stocksService;
            _configuration = configuration;
        }

        public async Task<IViewComponentResult> InvokeAsync(string stockSymbol)
        {
            Dictionary<string, object?>? companyProfile = await _stockService.FetchCompanyProfile(stockSymbol, _configuration.GetValue<string>("finnhubKey"));

            return View(companyProfile);
            //if (!string.IsNullOrEmpty(stockSymbol))
            //{
            //    Dictionary<string,object?>? companyProfile = await _stockService.FetchCompanyProfile(stockSymbol,_configuration.GetValue<string>("finnhubKey"));

            //    return View(companyProfile);
            //}
            //else
            //{
            //    ViewBag.ErrorMessage = "Stock symbol is missing or not right to fetch company profile.";
            //}

            // return View("AllStocks");
        }
    }
}
