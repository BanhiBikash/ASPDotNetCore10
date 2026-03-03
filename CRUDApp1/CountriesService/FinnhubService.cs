using System;
using System.Collections.Generic;
using System.Text;
using ServiceContracts;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FinnhubService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Dictionary<string, object>> GetCompanyProfile(string? stockSymbol, string? APIKey)
        {
            if(string.IsNullOrEmpty(stockSymbol))
            {
                throw new ArgumentNullException("Stock Symbol not delivered or invalid.");
            }

            if (string.IsNullOrEmpty(APIKey))
            {
                throw new ArgumentNullException("API Key not delivered or invalid.");
            }

            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={APIKey}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();
                    StreamReader streamReader = new StreamReader(stream);

                    string responseContent = await streamReader.ReadToEndAsync();

                    Dictionary<string, object>? companyProfile = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);

                    return companyProfile;
                }
                else
                {
                    return null;
                }

            }
        }

        public async Task<Dictionary<string, object>> GetStockInfo(string? stockSymbol, string? APIKey)
        {
            if (string.IsNullOrEmpty(stockSymbol))
            {
                throw new ArgumentNullException("Stock Symbol not delivered or invalid.");
            }

            if (string.IsNullOrEmpty(APIKey))
            {
                throw new ArgumentNullException("API Key not delivered or invalid.");
            }

            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={APIKey}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();
                    StreamReader streamReader = new StreamReader(stream);

                    string Response = await streamReader.ReadToEndAsync();

                    Dictionary<string, object>? stockInfo = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(Response);

                    return stockInfo;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
