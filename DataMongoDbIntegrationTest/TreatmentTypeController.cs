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
    public class TreatmentTypeController : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient _client;

        public TreatmentTypeController(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        [Theory]
        [InlineData("/admin/treatmenttype")]
        [InlineData("/admin/treatmenttype/5ef3b8825e3b112e5c423d52")]
        public async Task GetEndPoint_TreatmentType(string endpoint)
        {
            var response = await _client.GetAsync(endpoint);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteEndPoint_Success()
        {
            var request = new
            {
                Url = "/admin/treatmenttype",
                Body = new TreatmentTypeEntry(){
                   Type = "TestType"
                }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();
            var treatmentType = JsonConvert.DeserializeObject<TreatmentType>(value);

            await _client.DeleteAsync($"{request.Url}/{treatmentType.ID}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Post_NewTreatmentType_Existing_Return_BadRequest()
        {
            var request = new
            {
                Url = "/admin/treatmenttype",
                Body = new TreatmentTypeEntry()
                {
                    Type = "SNS"
                }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_NewTreatmentType_Success()
        {
            var request = new
            {
                Url = "/admin/treatmenttype",
                Body = new TreatmentTypeEntry()
                {
                    Type = "NewTreatmentType"
                }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();
            var treatmentType = JsonConvert.DeserializeObject<TreatmentType>(value);

            await _client.DeleteAsync($"{request.Url}/{treatmentType.ID}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}
