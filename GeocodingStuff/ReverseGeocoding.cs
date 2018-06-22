using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GeocodingStuff
{
    public class ReverseGeocoding
    {
        public IHttpHandler Client { get; set; } = new HttpHandler();
        
        public async Task<string> FetchAsync(Uri URL)
        {
            
            HttpResponseMessage response = Client.GetAsync(URL).Result;
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
        
        public async Task ProcessLocations(GeoUtil.Config config, IList<string> locations)
        {
//            Dictionary<string, string> revGeocodingResults = new Dictionary<string, string>();
            List<string> locationsRevGeocoded = new List<string>();
            
            foreach(var location in locations)
            {
                Uri url = new Uri(GeoUtil.GenerateUrlFromLatLong(config.UrlTemplate, location, config.ApiKey));
                string revGeocodingResult = await FetchAsync(url);
                
                // prepend the queried lat-long to the resultset -->
                string adjustedRevGeocodingResult = $"{{\r\n\"queried_location\":\"{location}\",\r\n" 
                                                    + revGeocodingResult.Substring(1);
                
//                revGeocodingResults.Add(location, revGeocodingResult);
                locationsRevGeocoded.Add(revGeocodingResult);
                File.AppendAllText(config.OutFile, "\r\n" + adjustedRevGeocodingResult);
            }            
        }
    }
}