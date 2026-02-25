using Microsoft.AspNetCore.Mvc;
using StocksApp.Services;

namespace StocksApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly FinnhubService _finnhubService;

        public HomeController(FinnhubService finnhubService)
        {
            _finnhubService = finnhubService;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            //StockData stockData = new StockData();
            Dictionary<string, object> ResponseDictionary = await _finnhubService.GetStockQuote();

            if (ResponseDictionary != null)
            {
                return View(ResponseDictionary);
            }
            else
            {
                return NotFound("No response from Finnhub.");
            }
        }
    }
}

