using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GeocodingStuff
{
    public static class GeoUtil
    {
        public const string DFLT_CONFIG_PATH = "config.json";
        
        public static IList<string> ReadLocationsFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File {path} not found.");
            }
            List<string> retVal = new List<string>(File.ReadAllLines(path));

            return retVal;
        }
        
        //        public static string GenerateUrlFromLatLong(string latLong, string apiKey) => $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latLong}&key={apiKey}";

        public static string GenerateUrlFromLatLong(string template, string latLong, string apiKey) =>
            string.Format(template, latLong, apiKey);

        public static void WriteResultAsJson(IDictionary<string,string> locations, string outputPath)
        {
            string json = JsonConvert.SerializeObject(locations, Formatting.Indented);
            File.WriteAllText(outputPath, json);
        }

        public static Config FillConfigFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File {path} not found");
            }
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
        }

        public class Config
        {
            public string ConfigFile { get; set; } = DFLT_CONFIG_PATH;
            public string ApiKey { get; set; }
            public string UrlTemplate { get; set; }
            public string InFile { get; set; }
            public string OutFile { get; set; }
        }
        
    }
}