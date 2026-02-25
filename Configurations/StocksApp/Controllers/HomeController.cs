using Microsoft.AspNetCore.Mvc;
using StocksApp.Services;
using StocksApp.Models;

namespace StocksApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly Stocks _stocks;

        public HomeController(Stocks stocks)
        {
            _stocks = stocks;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            //StockData stockData = new StockData();
            await _stocks.GetStocks();
            return View();
        }
    }
}
