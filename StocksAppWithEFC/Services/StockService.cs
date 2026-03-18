using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class StockService : IStockService
    {
        public Task<int> BuyStocks(BuyOrderRequest? buyOrderRequest)
        {
            throw new NotImplementedException();
        }

        public Task<int> SellStocks(SellOrderRequest? sellOrderRequest)
        {
            throw new NotImplementedException();
        }
    }
}
