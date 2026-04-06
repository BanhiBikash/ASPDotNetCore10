using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContracts
{
    public interface IStocksService
    {
        Task<Guid?> BuyStocks(BuyOrderRequest? buyOrderRequest);
        Task<Guid?> SellStocks(SellOrderRequest? sellOrderRequest);
        Task<Dictionary<string,object?>> FetchStockQuote(string? stockSymbol, string? Key);
        Task<List<BuyOrderResponse>?> GetBuyOrderList();
        Task<List<SellOrderResponse>?> GetSellOrderList();
        Task<Dictionary<string, object?>> FetchCompanyProfile(string stockSymbol, string Key);
    }
}
