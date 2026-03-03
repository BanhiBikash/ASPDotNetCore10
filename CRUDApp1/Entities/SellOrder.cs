using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class SellOrder
    {
        [Key]
        public Guid SellOrderID { get; set; }

        [Required(ErrorMessage = "Stock Symbol is mandatory")]
        public string? StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock Name is mandatory")]
        public string? StockName { get; set; }

        public DateTime? DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "Quantity must be between 1 and 100000")]
        public uint? Quantity { get; set; }

        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000")]
        public double? Price { get; set; }
    }
}
