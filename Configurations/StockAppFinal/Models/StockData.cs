namespace StockAppFinal.Models
{
    public class StockData
    {
        // Basic stock info
        public string StockName { get; set; }
        public string StockSymbol { get; set; }

        // Financial metrics
        public double CurrentPrice { get; set; }        // c
        public double Change { get; set; }              // d
        public double PercentChange { get; set; }       // dp
        public double HighPriceOfDay { get; set; }      // h
        public double LowPriceOfDay { get; set; }       // l
        public double OpenPriceOfDay { get; set; }      // o
        public double PreviousClosePrice { get; set; }  // pc
    }


}
