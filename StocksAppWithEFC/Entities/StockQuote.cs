using System.ComponentModel.DataAnnotations;

namespace StocksAppWithEFC.Models
{
    public class StockQuote
    {
        [Display(Name = "Current Price")]
        public decimal c { get; set; }

        [Display(Name = "Change")]
        public decimal d { get; set; }

        [Display(Name = "Percent Change")]
        public decimal dp { get; set; }

        [Display(Name = "High Price of the Day")]
        public decimal h { get; set; }

        [Display(Name = "Low Price of the Day")]
        public decimal l { get; set; }

        [Display(Name = "Open Price of the Day")]
        public decimal o { get; set; }

        [Display(Name = "Previous Close Price")]
        public decimal pc { get; set; }

        [Display(Name = "Timestamp")]
        public long t { get; set; }
    }
}
