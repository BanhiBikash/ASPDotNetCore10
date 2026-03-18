using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IStockService
    {
        Task<int> BuyStocks(BuyOrderRequest? buyOrderRequest);

        Task<int> SellStocks(SellOrderRequest? sellOrderRequest);
    }
}
