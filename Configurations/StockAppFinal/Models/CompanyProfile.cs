namespace StockAppFinal.Models
{
    public class CompanyProfile
    {
        // Country of company's headquarter
        public string Country { get; set; }

        // Currency used in company filings
        public string Currency { get; set; }

        // Listed exchange
        public string Exchange { get; set; }

        // Finnhub industry classification
        public string Industry { get; set; }

        // IPO date
        public DateTime Ipo { get; set; }

        // Logo image URL
        public string Logo { get; set; }

        // Market Capitalization
        public double MarketCapitalization { get; set; }

        // Company name
        public string Name { get; set; }

        // Company phone number
        public string Phone { get; set; }

        // Number of outstanding shares
        public double ShareOutstanding { get; set; }

        // Company symbol/ticker
        public string Ticker { get; set; }

        // Company website
        public string WebUrl { get; set; }
    }

}
