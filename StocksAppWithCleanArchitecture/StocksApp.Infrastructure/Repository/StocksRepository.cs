using StocksApp.Core.Entities;
using StocksApp.Core.RespositoryContract;
using StocksApp.Infrastructure.DBContext;

namespace StocksApp.Infrastructure.Repository
{
    public class StocksRepository : IStocksRepository
    {
        private readonly StocksDBContext _dbContext;

        public StocksRepository(StocksDBContext stocksDBContext)
        {
            _dbContext = stocksDBContext;
        }

        public async Task<bool> ConnectionCheck()
        {
            bool check = await _dbContext.Database.CanConnectAsync();

            if (check)
            {
                return check;
            }
            else
            {
                throw new InvalidOperationException("Database connection failed.");
            }
        }

        public async Task<Guid?> AddBuyOrder(BuyOrder buyOrder)
        {
            await ConnectionCheck();

            await _dbContext.BuyOrders.AddAsync(buyOrder);
            int recordsAdded = await _dbContext.SaveChangesAsync();

            if (recordsAdded > 0)
            {
                return buyOrder.BuyOrderID;
            }
            else
            {
                return null;
            }
        }

        public async Task<Guid?> AddSellOrder(SellOrder sellOrder)
        {
            await ConnectionCheck();

            await _dbContext.SellOrders.AddAsync(sellOrder);
            int recordsAddded = await _dbContext.SaveChangesAsync();

            if(recordsAddded > 0)
            {
                return sellOrder.SellOrderID;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<BuyOrder>?> FetchBuyOrders()
        {
            await ConnectionCheck();
            return _dbContext.BuyOrders.ToList();
        }

        public async Task<List<SellOrder>?> FetchSellOrders()
        {
            await ConnectionCheck();
            return _dbContext.SellOrders.ToList();
        }
    }
}
