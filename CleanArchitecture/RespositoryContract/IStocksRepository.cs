using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace RespositoryContract
{
    public interface IStocksRepository
    {
        Task<Guid?> AddBuyOrder(BuyOrder buyOrder);
        Task<Guid?> AddSellOrder(SellOrder sellOrder);
        Task<List<BuyOrder>?> FetchBuyOrders();
        Task<List<SellOrder>?> FetchSellOrders();
    }
}
