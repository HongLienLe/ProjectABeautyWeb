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
                    IsAddOn = false,
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
        [InlineData("/admin/treatement/5ec8579645e16549ec3afd7e")]
        public async Task GetEndpoint(string endpoint)
        {
            var response = await _client.GetAsync(endpoint);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetEnpoint_InvalidId()
        {
            var response = await _client.GetAsync("/admin/treatment/invalidid");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("/admin/treatement/5ec8579645e16549ec3afd7e")]
        public async Task PutEndpoint_Update_Treatment(string endpoint)
        {
            var body = new
            {
                TreatmentName = "TEST Test Test",
                TreatmentType = "Sns",
                IsAddOn = false,
                Price = 25,
                Duration = 45
            };

            var response = await _client.PutAsync(endpoint, ContentHelper.GetStringContent(body));

            var getTreatment = await _client.GetAsync(endpoint);
            var treatmentValue = await getTreatment.Content.ReadAsStringAsync();
            var treatment = JsonConvert.DeserializeObject<Treatment>(treatmentValue);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            Assert.NotNull(treatmentValue);
            Assert.Equal(treatment.About.Price, body.Price);
        }

        [Theory]
        [InlineData("/admin/treatment/invalidtreatmentid")]
        public async Task DeleteEndpoint_Treatment(string endpoint)
        {
            var response = await _client.DeleteAsync(endpoint);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
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
                    TreatmentType = "Sns",
                    IsAddOn = false,
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
            var treatmentRequest = new
            {
                Url = "/admin/treatment",
                Body = new
                {
                    TreatmentName = "Full set",
                    TreatmentType = "Acrylic",
                    IsAddOn = false,
                    Price = 22,
                }
            };

            var treatmentResponse = await _client.PostAsync(treatmentRequest.Url, ContentHelper.GetStringContent(treatmentRequest.Body));
            var treatmentValue = await treatmentResponse.Content.ReadAsStringAsync();
            var treatment = JsonConvert.DeserializeObject<Treatment>(treatmentValue);

            var request = new
            {
                Url = "/admin/employee/5ec9b64fecb17955b7b3be84/manage/treatment",
                Body = new[]
                {
                   $"{treatment.ID}"
                }
            };

            await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            var deleteResponse = await _client.DeleteAsync($"/admin/treatment/{treatment.ID}");

            var getEmployee = await _client.GetAsync("/admin/employee/5ec9b64fecb17955b7b3be84");
            var employeeValue = await getEmployee.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<Employee>(employeeValue);

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.DoesNotContain(treatment.ID, employee.Treatments);

        }
    }
}
