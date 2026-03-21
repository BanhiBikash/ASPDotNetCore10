using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;

namespace ServiceContracts.DTO
{
    public class SellOrderResponse
    {
        public Guid? SellOrderID { get; set; }

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
            obj is SellOrderResponse other &&
            SellOrderID == other.SellOrderID &&
            stockSymbol == other.stockSymbol &&
            stockName == other.stockName &&
            orderDate == other.orderDate &&
            orderQuantity == other.orderQuantity &&
            stockPrice == other.stockPrice;

        public override int GetHashCode() =>
            HashCode.Combine(SellOrderID, stockSymbol, stockName, orderDate, orderQuantity, stockPrice);
    }

    public static class SellOrderExtension
    {
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse() { SellOrderID = sellOrder.SellOrderID, stockSymbol = sellOrder.stockSymbol, stockName = sellOrder.stockName, orderDate = sellOrder.orderDate, orderQuantity = sellOrder.orderQuantity, stockPrice = sellOrder.stockPrice };
        }
    }
}
