using System.Net.Http;

namespace StocksApp.Services
{
    public class Stocks
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Stocks(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task GetStocks()
        {
            using (HttpClient httpclient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://finnhub.io/api/v1/quote?symbol=AAPL&token=d6fhve1r01qjq8n1niogd6fhve1r01qjq8n1nip0"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpclient.SendAsync(httpRequestMessage);

                if(httpResponseMessage.IsSuccessStatusCode)
                {
                    Stream stream = httpResponseMessage.Content.ReadAsStream();
                    StreamReader streamReader = new StreamReader(stream);

                    string response = streamReader.ReadToEnd();
                }
            }
        }
    }
}
