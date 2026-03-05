using Microsoft.AspNetCore.Mvc;
using Services;
using ServiceContracts;
using StocksAppWithUnitTest.Models;
using System.Text.Json;

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

                if(StockInfo == null || StockInfo.Count == 0)
                {
                    return NotFound("Stock information is not found.");
                }

                Stock stock = new Stock()
                {
                    StockName = CompanyName,
                    StockSymbol = CompanySymbol,
                    CurrentPrice = ((JsonElement)StockInfo["c"]).GetDecimal(),
                    Change = ((JsonElement)StockInfo["d"]).GetDecimal(),
                    PercentChange = ((JsonElement)StockInfo["dp"]).GetDecimal(),
                    HighPrice = ((JsonElement)StockInfo["h"]).GetDecimal(),
                    LowPrice = ((JsonElement)StockInfo["l"]).GetDecimal(),
                    OpenPrice = ((JsonElement)StockInfo["o"]).GetDecimal(),
                    PreviousClosePrice = ((JsonElement)StockInfo["pc"]).GetDecimal()
                };

                return View(stock);
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

                CompanyProfile CompanyData = new CompanyProfile()
                {
                    Country = ((JsonElement)companyInfo["country"]).GetString(),
                    Currency = ((JsonElement)companyInfo["currency"]).GetString(),
                    Exchange = ((JsonElement)companyInfo["exchange"]).GetString(),
                    Industry = ((JsonElement)companyInfo["finnhubIndustry"]).GetString(),
                    Ipo = ((JsonElement)companyInfo["ipo"]).GetDateTime(),
                    Logo = ((JsonElement)companyInfo["logo"]).GetString(),
                    MarketCapitalization = ((JsonElement)companyInfo["marketCapitalization"]).GetDouble(),
                    Name = ((JsonElement)companyInfo["name"]).GetString(),
                    Phone = ((JsonElement)companyInfo["phone"]).GetString(),
                    ShareOutstanding = ((JsonElement)companyInfo["shareOutstanding"]).GetDouble(),
                    Ticker = ((JsonElement)companyInfo["ticker"]).GetString(),
                    WebUrl = ((JsonElement)companyInfo["weburl"]).GetString()
                };

                return View(CompanyData);
            }
            else
            {
                return BadRequest("Either Stock Code or API key is not delivered.");
            }
        }
    }
}
