using Microsoft.AspNetCore.Mvc;
using StockAppFinal.Models;
using StockAppFinal.ServiceContracts;
using System.Text.Json;

namespace StockAppFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;

        public HomeController(IFinnhubService finnhubService, IConfiguration configuration)
        {
            _finnhubService = finnhubService;
            _configuration = configuration;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            string? APIKey = _configuration.GetValue<string>("APIData:APIKey");

            string stockSymbol = "AAPL";
            Dictionary<string, object>? ResponseDictionary = await _finnhubService.GetStockQuote(stockSymbol,APIKey);

            StockData stockData = new StockData()
            {
                StockName = "Microsoft",
                StockSymbol = stockSymbol,
                CurrentPrice = ((JsonElement)ResponseDictionary["c"]).GetDouble(),
                Change = ((JsonElement)ResponseDictionary["d"]).GetDouble(),
                PercentChange = ((JsonElement)ResponseDictionary["dp"]).GetDouble(),
                HighPriceOfDay = ((JsonElement)ResponseDictionary["h"]).GetDouble(),
                LowPriceOfDay = ((JsonElement)ResponseDictionary["l"]).GetDouble(),
                OpenPriceOfDay = ((JsonElement)ResponseDictionary["o"]).GetDouble(),
                PreviousClosePrice = ((JsonElement)ResponseDictionary["pc"]).GetDouble()
            };

            return View(stockData);
        }
    }
}
