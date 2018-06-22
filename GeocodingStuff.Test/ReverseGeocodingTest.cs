using System;
using System.Net;
using System.Net.Http;
using Xunit;
using NSubstitute;

namespace GeocodingStuff.Test
{
    public class ReverseGeocodingTest
    {
        [Fact]
        public void DummyTest()
        {
            Assert.True(true);
        }
        
        public class FetchAsyncTests
        {
            [Fact]
            public async void WhenUrlGivenResultFetchedTest()
            {
                string expectedResult = "{\"testkey\":\"testvalue\"}";

                ReverseGeocoding subject = new ReverseGeocoding();
                Uri testUri = new Uri("http://test.iets");
                HttpResponseMessage testResponse = new HttpResponseMessage(HttpStatusCode.OK);
                
                testResponse.Content = new StringContent(expectedResult);
                
                IHttpHandler client = Substitute.For<IHttpHandler>();
                client.GetAsync(testUri).Returns(testResponse);

                subject.Client = client;
                string actualResult = await subject.FetchAsync(testUri);

                Assert.Equal(expectedResult, actualResult);
            }

            [Fact]
            public async void WhenUrlGivenAndNotSuccesfulThenErrorReturned()
            {
                string expectedResult = "{\"results\": \"ERROR\"}";
                ReverseGeocoding subject = new ReverseGeocoding();
                Uri testUri = new Uri("http://test.iets");
                HttpResponseMessage testReponse = new HttpResponseMessage(HttpStatusCode.NotFound);

                IHttpHandler client = Substitute.For<IHttpHandler>();
                client.GetAsync(testUri).Returns(testReponse);

                subject.Client = client;
                string actualResult = await subject.FetchAsync(testUri);

                Assert.Equal(expectedResult, actualResult);
            }
        }
    }
}