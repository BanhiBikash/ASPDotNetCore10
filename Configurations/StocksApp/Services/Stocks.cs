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
                    RequestUri = new Uri("https://api.twelvedata.com/stocks?exchange=NYSE&format=JSON&apikey=demo"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpclient.SendAsync(httpRequestMessage);
            }
        }
    }
}
