using ServiceContracts;
using ServiceContracts.DTO;
using StocksAppWithEFC.Models;
using System.Reflection;
using System.Text.Json;
using Entities;
using Entities.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class StockService : IStockService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly StocksDBContext _stocksDBContext;

        public StockService(IHttpClientFactory httpClientFactory, StocksDBContext stocksDBContext)
        {
            _httpClientFactory = httpClientFactory;
            _stocksDBContext = stocksDBContext;
        }

        public async Task<bool> BuyStocks(BuyOrderRequest? buyOrderRequest)
        {
            //check if request is null
            if (buyOrderRequest == null) throw new ArgumentException("Order Request is null");

            //check if any argument in the request is null
            foreach (var property in typeof(BuyOrderRequest).GetProperties())
            {
                if (property.GetValue(buyOrderRequest) == null)
                {
                    throw new ArgumentNullException(property.Name + " is null.");
                }
            }

            //create buy order
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();

            int initialCount = _stocksDBContext.BuyOrders.Count();

            await _stocksDBContext.BuyOrders.AddAsync(buyOrder);
            await _stocksDBContext.SaveChangesAsync();

            int finalCount = _stocksDBContext.BuyOrders.Count();

            if (initialCount < finalCount)
            {
                return true;
            }

            return false;

        }

        public async Task<StockQuote> FetchStockQuote(string? stockSymbol, string? Key)
        {
            StockQuote? stockQuote = null;

            if (stockSymbol == null || Key == null) throw new ArgumentException("Either stock symbol or api key is null.");

            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage() 
                { 
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&resolution=1&from=1738655051&to=1738741451&token={Key}"),
                    Method = HttpMethod.Get,
                }; 

                HttpResponseMessage  httpResponseMessage = await httpClient.SendAsync(requestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {

                    Stream stream = httpResponseMessage.Content.ReadAsStream();

                    StreamReader streamReader = new StreamReader(stream);

                    string response = await streamReader.ReadToEndAsync();

                    Dictionary<string, JsonElement>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(response);

                    stockQuote = new StockQuote()
                    {
                        c = responseDictionary["c"].GetDecimal(),
                        d = responseDictionary["d"].GetDecimal(),
                        dp = responseDictionary["dp"].GetDecimal(),
                        h = responseDictionary["h"].GetDecimal(),
                        l = responseDictionary["l"].GetDecimal(),
                        o = responseDictionary["o"].GetDecimal(),
                        pc = responseDictionary["pc"].GetDecimal(),
                        t = responseDictionary["t"].GetInt64()
                    };
                }

            }
            return stockQuote;
        }

        public async Task<bool> SellStocks(SellOrderRequest? sellOrderRequest)
        {
            //check if request is null
            if (sellOrderRequest == null) throw new ArgumentException("Order Request is null");

            //check if any argument in the request is null
            foreach(var property in typeof(SellOrderRequest).GetProperties())
            {
                if(property.GetValue(sellOrderRequest) == null)
                {
                    throw new ArgumentNullException(property.Name + " is null.");
                }
            }

            //create sell order
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();

            int initialCount = _stocksDBContext.SellOrders.Count(); 

            await _stocksDBContext.SellOrders.AddAsync(sellOrder);
            await _stocksDBContext.SaveChangesAsync();

            int finalCount = _stocksDBContext.SellOrders.Count();

            if(initialCount < finalCount)
            {
                return true;
            }

            return false; 
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrderList()
        {
            List<BuyOrderResponse>? buyList = null;  

            buyList = await _stocksDBContext.BuyOrders.Select(order=>order.ToBuyOrderResponse()).ToListAsync();

            return buyList;
        }  

        public async Task<List<SellOrderResponse>> GetSellOrderList()
        {
            List<SellOrderResponse>? sellList = null;

            sellList = await _stocksDBContext.SellOrders.Select(orders=>orders.ToSellOrderResponse()).ToListAsync();

            return sellList;
        }
    }
}
