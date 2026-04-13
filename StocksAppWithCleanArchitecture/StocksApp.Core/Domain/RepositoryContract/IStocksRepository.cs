using StocksApp.Core.Entities;

namespace StocksApp.Core.RespositoryContract
{
    public interface IStocksRepository
    {
        Task<Guid?> AddBuyOrder(BuyOrder buyOrder);
        Task<Guid?> AddSellOrder(SellOrder sellOrder);
        Task<List<BuyOrder>?> FetchBuyOrders();
        Task<List<SellOrder>?> FetchSellOrders();
    }
}
