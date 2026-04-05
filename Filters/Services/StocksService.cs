using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using RespositoryContract;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly IHttpClientFactory _contextFactory;

        public StocksService(IStocksRepository stocksRepository, IHttpClientFactory httpContextFactory)
        {
            _stocksRepository = stocksRepository;
            _contextFactory = httpContextFactory;
        }

        public async Task<Guid?> BuyStocks(BuyOrderRequest? buyOrderRequest)
        {
            //request is null
            if(buyOrderRequest == null) throw new ArgumentNullException("Buy Order Request is null");

            //some attribute is null
            foreach(var property in typeof(BuyOrderRequest).GetProperties())
            {
                if(property.GetValue(buyOrderRequest) == null)
                {
                    throw new ArgumentException(nameof(property)+" is null.");
                }
            }

            //adding
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();
            
            return await _stocksRepository.AddBuyOrder(buyOrder);
        }

        public async Task<Guid?> SellStocks(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null) throw new ArgumentNullException("Sell order request is null");

            foreach(var property in typeof(SellOrderRequest).GetProperties())
            {
                if(property.GetValue(sellOrderRequest)==null)
                {
                    throw new ArgumentException($"{nameof(property)} is null in Sell order request");
                }
            }

            //selling request
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();

            return await _stocksRepository.AddSellOrder(sellOrder);
        }

        //Get stock data from users
        public async Task<Dictionary<string, object?>> FetchStockQuote(string? stockSymbol, string? Key)
        {
            using (HttpClient httpClient = _contextFactory.CreateClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage() 
                { 
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&resolution=1&from=1738655051&to=1738741451&token={Key}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);

                if(responseMessage.IsSuccessStatusCode)
                {
                    string responseContent = await responseMessage.Content.ReadAsStringAsync();
                    Dictionary<string, object?> stockQuote = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object?>>(responseContent);
                    return stockQuote;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<BuyOrderResponse>?> GetBuyOrderList()
        {
            List<BuyOrderResponse>? buyOrderList = new List<BuyOrderResponse>();

            foreach (BuyOrder buyOrder in await _stocksRepository.FetchBuyOrders())
            {
                buyOrderList.Add(buyOrder.ToBuyOrderResponse());
            }

            return buyOrderList;
        }

        public async Task<List<SellOrderResponse>?> GetSellOrderList()
        {
            List<SellOrderResponse>? sellOrderList = new List<SellOrderResponse>();

            foreach (SellOrder sellOrder in await _stocksRepository.FetchSellOrders())
            {
                sellOrderList.Add(sellOrder.ToSellOrderResponse());
            }

            return sellOrderList;
        }
    }
}
