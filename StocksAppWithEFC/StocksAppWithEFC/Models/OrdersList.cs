using Entities;

namespace StocksAppWithEFC.Models
{
    public class OrdersList
    {
        public List<BuyOrder>? BuyOrdersList { get; set; }
        public List<SellOrder>? SellOrdersList { get; set; }
    }
}
