using ServiceContracts.DTO;
using Entities;
using StocksAppWithEFC.Models;

namespace ServiceContracts
{
    public interface IStockService
    {
        Task<bool> BuyStocks(BuyOrderRequest? buyOrderRequest);
        Task<bool> SellStocks(SellOrderRequest? sellOrderRequest);
        Task<StockQuote> FetchStockQuote(string? stockSymbol, string? Key);
        Task<List<BuyOrderResponse>> GetBuyOrderList();
        Task<List<SellOrderResponse>> GetSellOrderList();
    }
}
