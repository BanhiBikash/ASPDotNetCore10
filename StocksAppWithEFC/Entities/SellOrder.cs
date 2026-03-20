using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities
{
    public class SellOrder
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
    }

}
