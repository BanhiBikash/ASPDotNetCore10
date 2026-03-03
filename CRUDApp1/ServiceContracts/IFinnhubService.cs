using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContracts
{
    public interface IFinnhubService
    {
        Task<Dictionary<string, object>> GetCompanyProfile(string? stockSymbol, string? APIKey);

        Task<Dictionary<string, object>> GetStockInfo(string stockSymbol, string APIKey);
    }
}
