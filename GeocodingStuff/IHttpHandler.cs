using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GeocodingStuff
{
    public interface IHttpHandler
    {
        HttpResponseMessage Get(Uri uri);
        HttpResponseMessage Post(Uri uri, HttpContent httpContent);
        Task<HttpResponseMessage> GetAsync(Uri uri);
        Task<HttpResponseMessage> PostAsync(Uri uri, HttpContent httpContent);
    }
}