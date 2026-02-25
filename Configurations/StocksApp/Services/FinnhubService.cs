using StocksApp.ServiceContracts;
using System.Net.Http;
using System.Text.Json;
namespace StocksApp.Services
{
    public class FinnhubService:IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FinnhubService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Dictionary<string, object>> GetStockQuote(string Symbol="AAPL")
        {
            using (HttpClient httpclient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={Symbol}&token=d6fhve1r01qjq8n1niogd6fhve1r01qjq8n1nip0"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpclient.SendAsync(httpRequestMessage);

                if(httpResponseMessage.IsSuccessStatusCode)
                {
                    Stream stream = httpResponseMessage.Content.ReadAsStream();
                    StreamReader streamReader = new StreamReader(stream);

                    string response = streamReader.ReadToEnd();

                    Dictionary<string,object>? ResponseDictionary = JsonSerializer.Deserialize<Dictionary<string,object>>(response);

                    return (ResponseDictionary);
                }

                else
                {
                    return null;
                }
            }

        }
    }
}
