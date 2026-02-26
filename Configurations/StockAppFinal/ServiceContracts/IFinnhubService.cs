namespace StockAppFinal.ServiceContracts
{
    public interface IFinnhubService
    {
        Task<Dictionary<string, object>> GetStockQuote(string Symbol, string APIKey);
    }
}
