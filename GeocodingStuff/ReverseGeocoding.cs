using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GeocodingStuff
{
    public class ReverseGeocoding
    {
        public static async Task<string> FetchAsync(Uri URL)
        {
            HttpClient client = new HttpClient();
            
            HttpResponseMessage response = client.GetAsync(URL).Result;
            string result = string.Empty;

//            var s = JsonConvert.DeserializeObject(result);

            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                result = "{\"results\": \"ERROR\"}";
            }

            return result;
        }

        public async Task<string> FetchAsync(string URL) => await FetchAsync(new Uri(URL));
    }
}