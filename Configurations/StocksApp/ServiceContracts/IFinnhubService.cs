namespace StocksApp.ServiceContracts
{
    public interface IFinnhubService
    {
        Task<Dictionary<string, object>>? GetStockQuote(string Symbol);
    }
}
