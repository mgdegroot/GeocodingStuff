using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using NSubstitute;
using GeocodingStuff;
using Newtonsoft.Json;

namespace GeocodingStuff.Test
{
    public class GeoUtilTest
    {
        static void writeTestfile(string path, string content)
        {
            File.WriteAllText(path, content);

        }
        
        [Fact]
        public void DummyTest()
        {
            Assert.True(true);
        }

        public class GenerateUrlFromLatLongTests
        {
            [Fact]
            public void WhenTemplateAndParamsGivenThenUrlReturnedTest()
            {
                string template = "https://test.placeholder/1-{0}-placeholder-2-{1}";
                string insertedLatlong = "0000.00,1111.11",
                    insertedApikey = "APIKEY";
            
                string expectedResult = string.Format(template, insertedLatlong, insertedApikey);

                string actualResult = GeoUtil.GenerateUrlFromLatLong(template, insertedLatlong, insertedApikey);

                Assert.Equal(expectedResult, actualResult);
            }
        }

        public class FillConfigFromFileTests
        {
            [Fact]
            public void WhenCalledThenConfigFilled()
            {
                string path = "testconfig.json";
                string testfileContent = @"{
  ""ApiKey"": ""test_api_key"",
  ""UrlTemplate"": ""https://maps.googleapis.com/maps/api/geocode/json?latlng={0}&key={1}"",
  ""InFile"":""/tmp/test/test_in.txt"",
  ""OutFile"": ""/tmp/test/test_out.json""
}";
                writeTestfile(path, testfileContent);
                
                GeoUtil.Config expectedConfig = new GeoUtil.Config()
                {
                    ApiKey = "test_api_key",
                    ConfigFile = GeoUtil.DFLT_CONFIG_PATH,
                    InFile = "/tmp/test/test_in.txt",
                    OutFile = "/tmp/test/test_out.json",
                    UrlTemplate = "https://maps.googleapis.com/maps/api/geocode/json?latlng={0}&key={1}",
                };

                GeoUtil.Config actualConfig = GeoUtil.FillConfigFromFile(path);
                    
                Assert.Equal(expectedConfig, actualConfig);
            }

            [Fact]
            public void WhenCalledAndFileNotExistsThenExceptionThrown()
            {
                string path = "nonexistent/nonexistent.json";
                string expectedMessage = $"File {path} not found.";
                FileNotFoundException fnfe =
                    Assert.Throws<FileNotFoundException>(() => GeoUtil.FillConfigFromFile(path));
                Assert.Equal(expectedMessage, fnfe.Message);
            }
        }

        public class ReadLocationsFromFileTests
        {
            [Fact]
            public void WhenCalledThenLocationsReturnedAsList()
            {
                string path = "testlocations.txt";
                
                IList<string> expectedLocations = new List<string>(new [] {"1234.56,65.4321","2345.67,76.5432","5432.22,22.1234"});
                
                string testContent = string.Join("\r\n", expectedLocations.ToArray());
                writeTestfile(path, testContent);

                IList<string> actualLocations = GeoUtil.ReadLocationsFromFile(path);

                Assert.Equal(expectedLocations, actualLocations);
            }

            [Fact]
            public void WhenCallendAndFileNotExistsThenExceptionThrown()
            {
                string path = "nonexistent/nonexistent.txt";
                string expectedMessage = $"File {path} not found.";
                FileNotFoundException fnfe =
                    Assert.Throws<FileNotFoundException>(() => GeoUtil.ReadLocationsFromFile(path));
                Assert.Equal(expectedMessage, fnfe.Message);
            }
        }

        public class WriteResultAsJsonTests
        {
            [Fact]
            public void WhenCalledThenJsonWritten()
            {
                Dictionary<string,string> test = new Dictionary<string, string>()
                {
                    {"a-key","a-value"},
                    {"b-key", "b-value"}
                };
                
                string expectedContent = JsonConvert.SerializeObject(test, Formatting.Indented);
                string path = "testout.json";

                GeoUtil.WriteResultAsJson(test, path);

                string actualContent = File.ReadAllText(path);
                
                Assert.True(File.Exists(path));
                Assert.Equal(expectedContent, actualContent);

            }
            
        }



    }
}