using Entities;
using ServiceContracts.DTO;

namespace StocksAppWithEFC.Models
{
    public class OrdersList
    {
        public List<BuyOrderResponse>? BuyOrdersList { get; set; }
        public List<SellOrderResponse>? SellOrdersList { get; set; }
    }
}
