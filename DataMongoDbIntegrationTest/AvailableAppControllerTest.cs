using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DataMongoApi;
using FluentAssertions;
using Xunit;

namespace DataMongoDbIntegrationTest
{
    public class AvailableAppControllerTest : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient _client;
        public AvailableAppControllerTest(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        [Theory]
        [InlineData("/availability")]
        public async Task GetEndpoint_App_Valid(string endpoint)
        {
            var requests = new []
            {
                new {datetime = new DateTime(2020, 07, 24),
                treatmentids = new[]
               {
                   "5ec8579645e16549ec3afd7e"
               } },

                new {datetime = new DateTime(2020, 07, 25),
                treatmentids = new[]
               {
                   "5ec8579645e16549ec3afd7e"
               } },

                new {datetime = new DateTime(2020, 07, 26),
                treatmentids = new[]
               {
                   "5ec8579645e16549ec3afd7e"
               } }
            };

            foreach(var request in requests)
            {
                var response = await _client.PostAsync(endpoint, ContentHelper.GetStringContent(request));
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Theory]
        [InlineData("/availability")]
        public async Task GetEndpoint_App_Valid_MultipleTreatments(string endpoint)
        {
            var requests = new[]
           {
                new {datetime = new DateTime(2020, 07, 24),
                treatmentids = new[]
               {
                   "5ec8579645e16549ec3afd7e",
                   "5eced47d4247020b72163d5f"
               } },

                new {datetime = new DateTime(2020, 07, 25),
                treatmentids = new[]
               {
                   "5ec8579645e16549ec3afd7e",
                   "5eced47d4247020b72163d5f"
               } },

                new {datetime = new DateTime(2020, 07, 26),
                treatmentids = new[]
               {
                   "5ec8579645e16549ec3afd7e",
                   "5eced47d4247020b72163d5f"
               } }
            };

            foreach (var request in requests)
            {
                var response = await _client.PostAsync(endpoint, ContentHelper.GetStringContent(request));
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        

    }
}
