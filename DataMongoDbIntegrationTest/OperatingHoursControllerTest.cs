using System;
using System.Net.Http;
using System.Threading.Tasks;
using DataMongoApi;
using DataMongoApi.Models;
using Newtonsoft.Json;
using Xunit;

namespace DataMongoDbIntegrationTest
{
    public class OperatingHoursControllerTest : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient _client;

        public OperatingHoursControllerTest(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        //[Fact]
        //public async Task PostEndpoint_ValidDay()
        //{
        //    var request = new
        //    {
        //        Url = "/admin/operatinghours",
        //        Body = new
        //        {
        //            Day = "sunday",
        //            OpeningHr = "10:00",
        //            ClosingHr = "19:00",
        //            isOpen = false
        //        }
        //    };

        //    var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
        //    var value = await response.Content.ReadAsStringAsync();
        //    var workDay = JsonConvert.DeserializeObject<OperatingHours>(value);

        //    response.EnsureSuccessStatusCode();
        //}

        [Theory]
        [InlineData("admin/operatinghours")]
        public async Task GetEndpoint(string endpoint)
        {
            var response = await _client.GetAsync(endpoint);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UpdateEndpoint()
        {
            var request = new
            {
                Url = "admin/operatinghours",
                Body = new
                {
                    Day = "Thursday",
                    OpeningHr = "10:00",
                    ClosingHr = "20:00",
                    isOpen = true
                }
            };

            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            response.EnsureSuccessStatusCode();
        }
    }
}
