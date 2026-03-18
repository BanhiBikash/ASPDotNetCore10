using System;
using System.Collections.Generic;
using System.Text;
using ServicesContracts.DTO;

namespace ServicesContracts
{
    public interface IStockService
    {
        Task<int> BuyStock(BuyOrderRequest? buyOrderRequest);

        Task<int> SellStock(SellOrderRequest? sellOrderRequest);
    }
}
