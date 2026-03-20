namespace StocksAppWithEFC.Models
{
    public class StockData
    {
        public string? stockName { get; set;  }
        public string? stockSymbol { get; set; }
        public decimal? stockPrice { get; set; } 
        public List<string>? SuccessMessages {  get; set; }
        public List<string>? ErrorMessages { get; set; }
        public string? orderAction { get; set; }
        public int? orderQuantity { get; set; }
    }
}
