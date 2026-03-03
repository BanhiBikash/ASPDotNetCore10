using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class BuyOrderResponse
    {
        [Key]
        public Guid BuyOrderID { get; set; }

        [Required(ErrorMessage = "Stock Symbol is mandatory")]
        public string StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock Name is mandatory")]
        public string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "Quantity must be between 1 and 100000")]
        public uint Quantity { get; set; }

        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000")]
        public double Price { get; set; }
    }

    public static class BuyOrderResponseExtensions
    {
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder ?? default(DateTime),
                Quantity = buyOrder.Quantity ?? 0,
                Price = buyOrder.Price ?? 0.0
            };
        }
    }
}
