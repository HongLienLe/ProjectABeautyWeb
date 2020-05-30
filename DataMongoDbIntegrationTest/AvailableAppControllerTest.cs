using System;
using System.Collections.Generic;
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
                   "5ed186269b7dd7208a4e0631"
               } },

                new {datetime = new DateTime(2020, 07, 25),
                treatmentids = new[]
               {
                   "5ec8579645e16549ec3afd7e",
                   "5ed186269b7dd7208a4e0631"
               } },

                new {datetime = new DateTime(2020, 07, 26),
                treatmentids = new[]
               {
                   "5ec8579645e16549ec3afd7e",
                   "5ed186269b7dd7208a4e0631"
               } }
            };

            foreach (var request in requests)
            {
                var response = await _client.PostAsync(endpoint, ContentHelper.GetStringContent(request));
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }

        }

        [Fact]
        public async Task GetEndpoint_When_Full()
        {
            var url = "/availability";
            var request = new
            {
                datetime = new DateTime(2020, 05, 30),
                treatmentids = new[]
                {
                    "5ec8579645e16549ec3afd7e"
                }
            };

            var response = await _client.PostAsync(url, ContentHelper.GetStringContent(request));
           
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        [Fact]
        public async Task Get_InvalidTreamtent()
        {
            var url = "/availability";

            var request = new
            {
                datetime = new DateTime(2020, 07, 24),
                treatmentids = new[]
                {
                    "5ec8579645e16549ec3afd7e",
                    "5ed186269b7dd7208a4e0631"
                }
            };

            var response = await _client.PostAsync(url, ContentHelper.GetStringContent(request));
            response.EnsureSuccessStatusCode();
        }

    }
}
