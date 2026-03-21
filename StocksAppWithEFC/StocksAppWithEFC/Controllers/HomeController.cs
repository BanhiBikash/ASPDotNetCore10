using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksAppWithEFC.Models;
using Rotativa.AspNetCore;

namespace StocksAppWithEFC.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public readonly IStockService _stockService;

        public HomeController(IConfiguration configuration, IStockService stockService)
        {
            _configuration = configuration;
            _stockService = stockService;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            string? stockSymbol = _configuration.GetValue<string>("StockData:StockName");
            string? Key = _configuration.GetValue<string>("StockData:Key");
            StockData? data;

            StockQuote? stockQuote = await  _stockService.FetchStockQuote(stockSymbol,Key);

            if(stockQuote != null)
            {
                data = new StockData() { stockSymbol = stockSymbol, stockName = "Microsoft", stockPrice = stockQuote.c, SuccessMessages = new List<string>() { "Stock Loaded successfully" }, ErrorMessages = null };
                return View(data);
            }
            else
            {
                data = new StockData() { stockSymbol = stockSymbol, stockName = "Microsoft", stockPrice = stockQuote.c, SuccessMessages = null, ErrorMessages = new List<string>() { "Failed to load stock" } };
                return View(data);
            }
        }

        [Route("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder(StockData? stockData)
        {
            StockData data;

            if (stockData.orderAction == null)
            {
                data = new StockData() { stockSymbol = stockData.stockSymbol, stockName = stockData.stockName, stockPrice = stockData.stockPrice, SuccessMessages = null, ErrorMessages = new List<string>() { "Failed to place order." } };
                return View("Index",data);
            }
            //sell order
            else if (stockData.orderAction == "sell")
            {

                if(!string.IsNullOrEmpty(stockData.stockName) && !string.IsNullOrEmpty(stockData.stockSymbol) && stockData.stockPrice!=null)
                {
                    SellOrderRequest sellRequest = new SellOrderRequest()
                    { 
                        stockName = stockData.stockName,
                        stockSymbol = stockData.stockSymbol,
                        stockPrice = stockData.stockPrice,
                        orderDate = DateTime.Now,
                        orderQuantity = stockData.orderQuantity
                    };

                    bool sellresponse = await _stockService.SellStocks(sellRequest);

                    if (sellresponse)
                    {
                        data = new StockData() { stockSymbol = stockData.stockSymbol, stockName = stockData.stockName, stockPrice = stockData.stockPrice, SuccessMessages = new List<string>() { $"{stockData.orderQuantity} stock/s of {stockData.stockName} has been successfully sold at a price of {stockData.stockPrice}" }, ErrorMessages = null };
                        return View("Index", data);
                    }
                }

                data = new StockData() { stockSymbol = stockData.stockSymbol, stockName = stockData.stockName, stockPrice = stockData.stockPrice, SuccessMessages = null, ErrorMessages = new List<string>() { "Failed to place sell order." } };
                return View(data);
            }
            //buy order
            else if (stockData.orderAction == "buy")
            {
                //if no argument is null
                if (!string.IsNullOrEmpty(stockData.stockName) && !string.IsNullOrEmpty(stockData.stockSymbol) && stockData.stockPrice != null)
                {
                    BuyOrderRequest buyRequest = new BuyOrderRequest()
                    {
                        stockName = stockData.stockName,
                        stockSymbol = stockData.stockSymbol,
                        stockPrice = stockData.stockPrice,
                        orderDate = DateTime.Now,
                        orderQuantity = stockData.orderQuantity
                    };

                    bool sellresponse = await _stockService.BuyStocks(buyRequest);

                    if (sellresponse)
                    {
                        data = new StockData() { stockSymbol = stockData.stockSymbol, stockName = stockData.stockName, stockPrice = stockData.stockPrice, SuccessMessages = new List<string>() { $"{stockData.orderQuantity} stock/s of {stockData.stockName} has been successfully bought at a price of {stockData.stockPrice}" }, ErrorMessages = null };
                        return View("Index", data);
                    }
                }

                data = new StockData() { stockSymbol = stockData.stockSymbol, stockName = stockData.stockName, stockPrice = stockData.stockPrice, SuccessMessages = null, ErrorMessages = new List<string>() { "Failed to place buy order." } };
                return View(data);
            }

            data = new StockData() { stockSymbol = stockData.stockSymbol, stockName = stockData.stockName, stockPrice = stockData.stockPrice, SuccessMessages = null, ErrorMessages = new List<string>() { "Failed to place order." } };
            return View(data);
        }


        [Route("Orders")]
        public async Task<IActionResult> Orders()
        {
            OrdersList? ordersList = new OrdersList() { BuyOrdersList = await _stockService.GetBuyOrderList(), SellOrdersList = await _stockService.GetSellOrderList() };

            return View(ordersList);
        }

        [Route("PrintPDF")]
        public async Task<IActionResult> PrintPDF()
        {
            OrdersList? ordersList = new OrdersList() { BuyOrdersList = await _stockService.GetBuyOrderList(), SellOrdersList = await _stockService.GetSellOrderList() };

            return new ViewAsPdf("PrintPDF",ordersList, ViewData);
        }
    }
}
