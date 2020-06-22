﻿using System;
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
    public class TreatmentControllerTest : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient _client;

        public TreatmentControllerTest(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task PostEndpoint_ValidTreatment()
        {
            var request = new
            {
                Url = "/admin/treatment",
                Body = new
                {
                    TreatmentName = "Full set",
                    TreatmentType = "Acrylic",
                    AddOn = false,
                    Price = 22,
                    Duration = 45
                }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();
            var treatment = JsonConvert.DeserializeObject<Treatment>(value);


            await _client.DeleteAsync($"/admin/treatment/{treatment.ID}");
            response.EnsureSuccessStatusCode();
            Assert.NotNull(treatment);
            Assert.Equal(treatment.About.TreatmentName, request.Body.TreatmentName);
        }


        [Theory]
        [InlineData("/admin/treatment")]
        [InlineData("/admin/treatment/5eecc6790fcc0e79a1973bb9")]
        public async Task GetEndpoint(string endpoint)
        {
            var response = await _client.GetAsync(endpoint);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetEnpoint_InvalidId()
        {
            var response = await _client.GetAsync("/admin/treatment/5ec8579645e16549ec3afd7A");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("/admin/treatment/5eecc67a0fcc0e79a1973bba")]
        public async Task PutEndpoint_Update_Treatment(string endpoint)
        {
            var body = new
            {
                TreatmentName = "TEST Test Test",
                TreatmentType = "SNS",
                AddOn = false,
                Price = 25,
                Duration = 45
            };

            var response = await _client.PutAsync(endpoint, ContentHelper.GetStringContent(body));

            var getTreatment = await _client.GetAsync(endpoint);
            var treatmentValue = await getTreatment.Content.ReadAsStringAsync();
            var treatment = JsonConvert.DeserializeObject<Treatment>(treatmentValue);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.Equal(treatment.About.Price, body.Price);
        }

        [Theory]
        [InlineData("/admin/treatment/5ec8579645e16549ec3afd7A")]
        public async Task DeleteEndpoint_Treatment_Invalid(string endpoint)
        {
            var response = await _client.DeleteAsync(endpoint);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteEndpoint_Valid()
        {
            var request = new
            {
                Url = "/admin/treatment",
                Body = new
                {
                    TreatmentName = "Full set",
                    TreatmentType = "SNS",
                    AddOn = false,
                    Price = 22,
                    Duration = 45
                }
            };

            var newTreatment = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var treatmentValue = await newTreatment.Content.ReadAsStringAsync();
            var treatment = JsonConvert.DeserializeObject<Treatment>(treatmentValue);

            var response = await _client.DeleteAsync($"/admin/treatment/{treatment.ID}");

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeletedTreatment_RemoveFrom_Employees()
        {

            var newTreatmentrequest = new
            {
                Url = "/admin/treatment",
                Body = new
                {
                    TreatmentName = "Full set",
                    TreatmentType = "SNS",
                    AddOn = false,
                    Price = 22,
                    Duration = 45
                }
            };

            var newTreatment = await _client.PostAsync(newTreatmentrequest.Url, ContentHelper.GetStringContent(newTreatmentrequest.Body));
            var treatmentValue = await newTreatment.Content.ReadAsStringAsync();
            var treatment = JsonConvert.DeserializeObject<Treatment>(treatmentValue);


            var request = new
            {
                Url = "/admin/employee/5eebea76755868745e0af6d3/manage/treatment",
                Body = new[]
                {
                   $"{treatment.ID}"
                }
            };


            await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            var deleteResponse = await _client.DeleteAsync($"/admin/treatment/{treatment.ID}");

            var getEmployee = await _client.GetAsync("/admin/employee/5eebea76755868745e0af6d3");
            var employeeValue = await getEmployee.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<Employee>(employeeValue);

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            //  Assert.DoesNotContain(treatment.ID, employee.Treatments);

        }
    }
}
