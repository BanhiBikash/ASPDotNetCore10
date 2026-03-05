using System.ComponentModel;

namespace StocksAppWithUnitTest.Models
{
    public class Stock
    {
        public string? StockName { get; set; }

        public string? StockSymbol { get; set; }

        /// <summary>
        /// Current price
        /// </summary>
        [DisplayName("Current Price")]
        public decimal CurrentPrice { get; set; }

        /// <summary>
        /// Change
        /// </summary>
        public decimal Change { get; set; }

        /// <summary>
        /// Percent change
        /// </summary>
        public decimal PercentChange { get; set; }

        /// <summary>
        /// High price of the day
        /// </summary>
        public decimal HighPrice { get; set; }

        /// <summary>
        /// Low price of the day
        /// </summary>
        public decimal LowPrice { get; set; }

        /// <summary>
        /// Open price of the day
        /// </summary>
        public decimal OpenPrice { get; set; }

        /// <summary>
        /// Previous close price
        /// </summary>
        public decimal PreviousClosePrice { get; set; }
    }
}
