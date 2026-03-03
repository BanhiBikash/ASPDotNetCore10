using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServiceContracts.DTO
{
    public class BuyOrderRequest
    {
        [Required(ErrorMessage = "Stock Symbol is mandatory")] public string StockSymbol { get; set; }
        [Required(ErrorMessage = "Stock Name is mandatory")] public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000, ErrorMessage = "Quantity must be between 1 and 100000")] public uint Quantity { get; set; }
        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000")] public double Price { get; set; }

        public BuyOrder ToBuyOrder()
        {
            return new BuyOrder
            {
                StockSymbol = this.StockSymbol,
                StockName = this.StockName,
                DateAndTimeOfOrder = this.DateAndTimeOfOrder,
                Quantity = this.Quantity,
                Price = this.Price
            };
        }
    }
}
