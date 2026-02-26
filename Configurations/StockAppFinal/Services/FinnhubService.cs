using Microsoft.Extensions.Configuration;
using StockAppFinal.ServiceContracts;
using System.Text.Json; 

namespace StockAppFinal.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FinnhubService(IHttpClientFactory httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Dictionary<string, object>> GetStockQuote(string Symbol = "AAPL", string APIKey = "d6fhve1r01qjq8n1niogd6fhve1r01qjq8n1nip0")
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage() 
                { 
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={Symbol}&token={APIKey}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if(httpResponseMessage.IsSuccessStatusCode)
                {
                    Stream stream = httpResponseMessage.Content.ReadAsStream();
                    StreamReader streamReader = new StreamReader(stream);

                    string Response = streamReader.ReadToEnd();

                    Dictionary<string, object>? stockQuote = JsonSerializer.Deserialize<Dictionary<string,object>>(Response);

                    return stockQuote;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
