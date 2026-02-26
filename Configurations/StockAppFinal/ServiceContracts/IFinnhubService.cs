namespace StockAppFinal.ServiceContracts
{
    public interface IFinnhubService
    {
        Task<Dictionary<string, object>> GetStockQuote(string Symbol, string APIKey);
        Task<Dictionary<string, object>> GetCompanyProfile(string Symbol, string APIKey);
    }
}
