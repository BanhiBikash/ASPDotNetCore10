using System;
using System.Collections.Generic;
using System.Text;
using ServicesContracts.DTO;
using ServicesContracts;

namespace Services
{
    public class StockService : IStockService
    {
        public Task<int> BuyStock(BuyOrderRequest? buyOrderRequest)
        {
            throw new NotImplementedException();
        }

        public Task<int> SellStock(SellOrderRequest? sellOrderRequest)
        {
            throw new NotImplementedException();
        }
    }
}
