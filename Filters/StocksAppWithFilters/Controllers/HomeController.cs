using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksAppWithFilters.Filters.ActionFilters;
using StocksAppWithFilters.Models;
using System.Text.Json;
using Rotativa.AspNetCore;

namespace StocksAppWithFilters.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly IStocksService _stocksService;
        private readonly IConfiguration _configuration;
        public HomeController(IStocksService stockService, IConfiguration configuration)
        {
            _stocksService = stockService;
            _configuration = configuration;
        }

        [Route("[Action]")]
        [Route("/")]
        public async Task<IActionResult> AllStocks()
        {
            string? finnhubKey = _configuration.GetValue<string>("finnhubKey");
            string[]? top25Stocks = _configuration.GetSection("StockData:Top25Stocks").Get<string[]>();

            return View(top25Stocks);
        }

        [TypeFilter(typeof(IndexActionFilter))]
        public async Task<IActionResult> Index()
        {
            string? companyName = _configuration.GetValue<string>("StockData:StockName");
            string? companySymbol = _configuration.GetValue<string>("StockData:StockSymbol");
            string? finnhubKey = _configuration.GetValue<string>("finnhubKey");
            StockData? stockData = null;
            Dictionary<string, object?>? stockQuote = await _stocksService.FetchStockQuote(companySymbol, finnhubKey);

            if (stockQuote == null)
            {
                stockData = new StockData()
                {
                    stockName = companyName,
                    stockSymbol = companySymbol,
                    stockPrice = 0,
                    ErrorMessages = new List<string> { "Failed to fetch stock quote data." }
                };

            }
            else
            {
                stockData = new StockData
                {
                    stockName = companyName,
                    stockSymbol = companySymbol,
                    stockPrice = stockQuote.ContainsKey("c") && stockQuote["c"] is JsonElement elem ? elem.GetDecimal() : 0m,
                    SuccessMessages = new List<string> { "Stock quote data fetched successfully." }
                };
            }

            return View(stockData);
        }

        [Route("PlaceOrder")]
        [TypeFilter(typeof(PlaceOrderActionFilter))]
        public async Task<IActionResult> PlaceOrder(StockData stockData)
        {

            if (stockData.orderAction == "buy")
            {
                BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
                {
                    stockName = stockData.stockName,
                    stockSymbol = stockData.stockSymbol,
                    stockPrice = stockData.stockPrice,
                    orderDate = DateTime.Now,
                    orderQuantity = stockData.orderQuantity
                };

                Guid? buyOrderID = await _stocksService.BuyStocks(buyOrderRequest);

                if (buyOrderID != null)
                {
                    StockData responseStockData = new StockData()
                    {
                        stockName = stockData.stockName,
                        stockSymbol = stockData.stockSymbol,
                        stockPrice = stockData.stockPrice,
                        SuccessMessages = new List<string>() { $"Buy order with order no. {buyOrderID} for {stockData.orderQuantity} stocks of {stockData.stockName} has been successfully placed." }
                    };
                }
                else
                {
                    StockData responseStockData = new StockData()
                    {
                        stockName = stockData.stockName,
                        stockSymbol = stockData.stockSymbol,
                        stockPrice = stockData.stockPrice,
                        ErrorMessages = new List<string>() { "Failed to place buy order" }
                    };
                }

                return View("Index", stockData);
            }
            else if (stockData.orderAction == "sell")
            {
                SellOrderRequest sellOrderRequest = new SellOrderRequest()
                {
                    stockName = stockData.stockName,
                    stockSymbol = stockData.stockSymbol,
                    stockPrice = stockData.stockPrice,
                    orderDate = DateTime.Now,
                    orderQuantity = stockData.orderQuantity
                };

                Guid? sellOrderID = await _stocksService.SellStocks(sellOrderRequest);

                if (sellOrderID != null)
                {
                    StockData responseStockData = new StockData()
                    {
                        stockName = stockData.stockName,
                        stockSymbol = stockData.stockSymbol,
                        stockPrice = stockData.stockPrice,
                        SuccessMessages = new List<string>()
        {
            $"Sell order with order no. {sellOrderID} for {stockData.orderQuantity} stocks of {stockData.stockName} has been successfully placed."
        }
                    };

                    return View("Index", responseStockData);
                }
                else
                {
                    StockData responseStockData = new StockData()
                    {
                        stockName = stockData.stockName,
                        stockSymbol = stockData.stockSymbol,
                        stockPrice = stockData.stockPrice,
                        ErrorMessages = new List<string>()
        {
            "Failed to place sell order"
        }
                    };

                    return View("Index", responseStockData);
                }

            }
            else
            {
                return View("Index", new StockData() { stockName = stockData.stockName, stockSymbol = stockData.stockSymbol, stockPrice = stockData.stockPrice, ErrorMessages = new List<string>() { "Please specify whether to buy or sell." } });
            }
        }

        [Route("Orders")]
        [TypeFilter(typeof(OrderActionFilter))]
        public async Task<IActionResult> Orders()
        {
            OrdersList? ordersList = new OrdersList() 
            { 
                BuyOrdersList = await _stocksService.GetBuyOrderList(),
                SellOrdersList = await _stocksService.GetSellOrderList()
            };

            return View(ordersList);
        }

        [Route("[Action]")]
        [TypeFilter(typeof(OrderActionFilter))]
        public async Task<IActionResult> PrintPDF()
        {
            OrdersList? ordersList = new OrdersList()
            {
                BuyOrdersList = await _stocksService.GetBuyOrderList(),
                SellOrdersList = await _stocksService.GetSellOrderList()
            };

            return new ViewAsPdf("Orders",ordersList,ViewData);
        }
    }
}
