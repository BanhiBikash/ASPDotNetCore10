using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServiceContracts.DTO
{
    public class BuyOrderResponse
    {
        public Guid? BuyOrderID { get; set; }

        [Required(ErrorMessage = "Stock Symbol can't be blank")]
        public string? stockSymbol { get; set; }

        [Required(ErrorMessage = "Stock Name can't be blank")]
        public string? stockName { get; set; }

        [Required(ErrorMessage = "Date can't be blank")]
        public DateTime? orderDate { get; set; }

        [Required(ErrorMessage = "Order Quantity is required")]
        [Range(1, 10000, ErrorMessage = "Order Quantity must be between {0} and {1}")]
        public int? orderQuantity { get; set; }

        [Required(ErrorMessage = "Stock Price is required")]
        [Range(1, 10000, ErrorMessage = "Stock Price must be between {0} and {1}")]
        public decimal? stockPrice { get; set; }

        //equals
        public override bool Equals(object obj) =>
       obj is BuyOrderResponse other &&
       BuyOrderID == other.BuyOrderID &&
       stockSymbol == other.stockSymbol &&
       stockName == other.stockName &&
       orderDate == other.orderDate &&
       orderQuantity == other.orderQuantity &&
       stockPrice == other.stockPrice;

        public override int GetHashCode() =>
            HashCode.Combine(BuyOrderID, stockSymbol, stockName, orderDate, orderQuantity, stockPrice);
    }
    public class BuyOrderExtension
    {
        public static BuyOrderResponse ToBuyOrderResponse(BuyOrder buyOrder)
        {
            return new BuyOrderResponse() { BuyOrderID = buyOrder.BuyOrderID, stockSymbol = buyOrder.stockSymbol, stockName = buyOrder.stockName, orderDate = buyOrder.orderDate, orderQuantity = buyOrder.orderQuantity, stockPrice = buyOrder.stockPrice };
        }
    }

}
