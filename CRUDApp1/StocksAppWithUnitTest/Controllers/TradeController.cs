using Microsoft.AspNetCore.Mvc;
using Services;
using ServiceContracts;
using StocksAppWithUnitTest.Models;

namespace StocksAppWithUnitTest.Controllers
{
    public class TradeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IFinnhubService _finnhubService;

        public TradeController(IConfiguration configuration, IFinnhubService finnhubService)
        {
            _configuration = configuration;
            _finnhubService = finnhubService;
        }
        
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            string? CompanySymbol = _configuration.GetValue<string>("TradingOptions:DefaultStockSymbol");
            string? CompanyName = _configuration.GetValue<string>("TradingOptions:DefaultStockName");
            string? MyAPIKey = _configuration.GetValue<string>("MyAPIKey");

            if (CompanySymbol != null && MyAPIKey != null)
            {
               Dictionary<string,object> StockInfo = await _finnhubService.GetStockInfo(CompanySymbol, MyAPIKey);

                Stock stock = new Stock()
                {
                    StockName = CompanyName,
                    StockSymbol = CompanySymbol,
                    CurrentPrice = Convert.ToDecimal(StockInfo["c"]),
                    Change = Convert.ToDecimal(StockInfo["d"]),
                    PercentChange = Convert.ToDecimal(StockInfo["dp"]),
                    HighPrice = Convert.ToDecimal(StockInfo["h"]),
                    LowPrice = Convert.ToDecimal(StockInfo["l"]),
                    OpenPrice = Convert.ToDecimal(StockInfo["o"]),
                    PreviousClosePrice = Convert.ToDecimal(StockInfo["pc"])
                };

                return View(StockInfo);
            }
            else
            {
                return BadRequest("Either Stock Code or API key is not delivered.");
            }
        }

        [Route("/CompanyInfo/{StockSymbol}")]
        public async Task<IActionResult> CompanyInfo()
        {
            string? CompanySymbol = HttpContext.Request.RouteValues["StockSymbol"].ToString();
            string? MyAPIKey = _configuration.GetValue<string>("MyAPIKey");

            if (CompanySymbol != null && MyAPIKey != null)
            {
                Dictionary<string, object> companyInfo = await _finnhubService.GetCompanyProfile(CompanySymbol, MyAPIKey);

                CompanyProfile stock = new CompanyProfile()
                {
                    Country = companyInfo["country"]?.ToString(),
                    Currency = companyInfo["currency"]?.ToString(),
                    Exchange = companyInfo["exchange"]?.ToString(),
                    Industry = companyInfo["finnhubIndustry"]?.ToString(),
                    Ipo = Convert.ToDateTime(companyInfo["ipo"]),
                    Logo = companyInfo["logo"]?.ToString(),
                    MarketCapitalization = Convert.ToDouble(companyInfo["marketCapitalization"]),
                    Name = companyInfo["name"]?.ToString(),
                    Phone = companyInfo["phone"]?.ToString(),
                    ShareOutstanding = Convert.ToDouble(companyInfo["shareOutstanding"]),
                    Ticker = companyInfo["ticker"]?.ToString(),
                    WebUrl = companyInfo["weburl"]?.ToString()
                };

                return View(companyInfo);
            }
            else
            {
                return BadRequest("Either Stock Code or API key is not delivered.");
            }
        }
    }
}
