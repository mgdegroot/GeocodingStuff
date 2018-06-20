using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GeocodingStuff
{
    /**
     * Simple reverse geocoder. For now implemented against Google reverse geocoding API.
     * Supply a API key and an URL template in the config.json.
     * TODO: since this is for now throwaway code a lot:
     * #1: adding command line options
     * #2: error handling
     * #3: unit tests
     * #x: multiple geocoding providers
     * #x: multiple input / output options
     */
    class Program
    {

        private static GeoUtil.Config _config;
        
        static async Task Main(string[] args)
        {
            _config = GeoUtil.FillConfigFromFile(GeoUtil.DFLT_CONFIG_PATH);
            
            IList<string> locations = GeoUtil.ReadLocationsFromFile(_config.InFile);

            processLocations(locations);
        }

        static async Task processLocations(IList<string> locations)
        {
            Dictionary<string, string> revGeocodingResults = new Dictionary<string, string>();
            List<string> locationsRevGeocoded = new List<string>();
            
            foreach(var location in locations)
            {
                Uri url = new Uri(GeoUtil.GenerateUrlFromLatLong(_config.UrlTemplate, location, _config.ApiKey));
                string revGeocodingResult = await ReverseGeocoding.FetchAsync(url);
                
                // prepend the queried lat-long to the resultset -->
                string adjustedRevGeocodingResult = $"{{\r\n\"queried_location\":\"{location}\",\r\n" 
                                                    + revGeocodingResult.Substring(1);
                
                revGeocodingResults.Add(location, revGeocodingResult);
                locationsRevGeocoded.Add(revGeocodingResult);
                File.AppendAllText(_config.OutFile, "\r\n" + adjustedRevGeocodingResult);
            }            
        }
    }
}