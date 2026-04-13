using StocksApp.Core.DTO;

namespace StocksApp.UI.Models
{
    public class OrdersList
    {
        public List<BuyOrderResponse>? BuyOrdersList { get; set; }
        public List<SellOrderResponse>? SellOrdersList { get; set; }
    }
}