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
    public class ClientControllerTest : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient _client;

        public ClientControllerTest(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task PostEndpoint_Create_ValidClient()
        {
            var request = new
            {
                Url = "/admin/client",
                Body = new
                {
                    FirstName = "FirstName",
                    LastName = "LastName",
                    Email = "Email@mail.com",
                    Phone = "12345678901"
                }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();
            var client = JsonConvert.DeserializeObject<Client>(value);

            await _client.DeleteAsync($"{request.Url}/{client.ID}");


            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.NotNull(client);
            Assert.Equal(request.Body.FirstName, client.About.FirstName);
        }

        [Fact]
        public async Task PostEndpoint_Create_InvalidClient()
        {
            var request = new
            {
                Url = "/admin/client",
                Body = new
                {
                    FirstName = "FirstNameiasdasdadasdasdasdasdsadsdsadasdsadsdadasdsdasd",
                    LastName = "LastNamasdsadsadsadsadsadsadsadsadsadsadsadsadsadsadasdasde",
                    Email = "Emailmail.com",
                    Phone = "123"
                }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var values = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("/admin/client")]
        [InlineData("/admin/client/5ecc38d0fa31a5694cbe8aed")]
        public async Task GetEndpoint_ReadClient(string endpoint)
        {
            var response = await _client.GetAsync(endpoint);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetEndpoint_Invalid()
        {
            var url = "/admin/client/invalidclient";

            var response = await _client.GetAsync(url);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PutEndpoint_Valid()
        {
            var newClientRequest = new
            {
                Url = "/admin/client",
                Body = new
                {
                    FirstName = "Test",
                    LastName = "Test",
                    Email = "Email@mail.com",
                    Phone = "12345678901"
                }
            };

            var newClientResponse = await _client.PostAsync(newClientRequest.Url, ContentHelper.GetStringContent(newClientRequest.Body));
            var value = await newClientResponse.Content.ReadAsStringAsync();
            var client = JsonConvert.DeserializeObject<Client>(value);

            var updatedClientDetails = new
            {
                FirstName = "Update",
                LastName = "Update",
                Email = "Email@mail.com",
                Phone = "12345678901"
            };


            var updateResponse = await _client.PutAsync($"{newClientRequest.Url}/{client.ID}", ContentHelper.GetStringContent(updatedClientDetails));

            var getUpdatedClient = await _client.GetAsync(newClientRequest.Url);
            var updatedValue = await getUpdatedClient.Content.ReadAsStringAsync();
            var clients = JsonConvert.DeserializeObject<IEnumerable<Client>>(updatedValue);

            await _client.DeleteAsync($"{newClientRequest.Url}/{client.ID}");

            Assert.Contains(clients, c => c.About.FirstName == updatedClientDetails.FirstName);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PutEndpoint_Invalid()
        {
            var newClientRequest = new
            {
                Url = "/admin/client",
                Body = new
                {
                    FirstName = "Test",
                    LastName = "Test",
                    Email = "Email@mail.com",
                    Phone = "12345678901"
                }
            };

            var newClientResponse = await _client.PostAsync(newClientRequest.Url, ContentHelper.GetStringContent(newClientRequest.Body));
            var value = await newClientResponse.Content.ReadAsStringAsync();
            var client = JsonConvert.DeserializeObject<Client>(value);

            var updatedClientDetails = new
            {
                FirstName = "Update",
                LastName = "Update",
                Email = "Email@mail.com",
                Phone = "123"
            };


            var updateResponse = await _client.PutAsync($"{newClientRequest.Url}/{client.ID}", ContentHelper.GetStringContent(updatedClientDetails));
            await _client.DeleteAsync($"{newClientRequest.Url}/{client.ID}");

            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        //[Fact]
        //public async Task DeleteEndpoint_Vaild()
        //{

        //}

        //[Fact]
        //public async Task DeleteEndpoint_Invalid()
        //{

        //}

        //[Fact]
        //public async Task DeleteClient_AndExistApp()
        //{

        //}
    }
}
