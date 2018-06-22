using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GeocodingStuff
{
    public class HttpHandler : IHttpHandler
    {
        private HttpClient _client = new HttpClient();


        public HttpResponseMessage Get(Uri uri)
        {
            return GetAsync(uri).Result;
        }

        public HttpResponseMessage Post(Uri uri, HttpContent httpContent)
        {
            return PostAsync(uri, httpContent).Result;
        }

        public async Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            return await _client.GetAsync(uri);
        }

        public async Task<HttpResponseMessage> PostAsync(Uri uri, HttpContent httpContent)
        {
            return await _client.PostAsync(uri, httpContent);
        }
    }
}