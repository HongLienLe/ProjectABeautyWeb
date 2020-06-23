using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DataMongoApi;
using DataMongoApi.Models;
using FluentAssertions;
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

        [Theory]
        [InlineData("admin/operatinghours")]
        [InlineData("admin/operatinghours/monday")]
        [InlineData("admin/operatinghours/tuesday")]
        [InlineData("admin/operatinghours/wednesday")]
        [InlineData("admin/operatinghours/thursday")]
        [InlineData("admin/operatinghours/friday")]
        [InlineData("admin/operatinghours/saturday")]
        [InlineData("admin/operatinghours/sunday")]
        public async Task GetEndpoint(string endpoint)
        {
            var response = await _client.GetAsync(endpoint);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateEndpoint()
        {
            var request = new
            {
                Url = "admin/operatinghours/thursday",
                Body = new
                {
                    Day = "thursday",
                    OpeningHr = "10:00",
                    ClosingHr = "20:00",
                    isOpen = true
                }
            };

            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            var updateResponse = await _client.GetAsync(request.Url);
            var readUpdate = await updateResponse.Content.ReadAsStringAsync();
            var day = JsonConvert.DeserializeObject<OperatingHours>(readUpdate);

            Assert.Contains(request.Body.ClosingHr, day.About.ClosingHr);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        //update, is opening time greater then closing time

        //timespan needs to be in the correct formatt
    }
}
