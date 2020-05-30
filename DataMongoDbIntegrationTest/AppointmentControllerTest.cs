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
    public class AppointmentControllerTest : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient _client;
        public AppointmentControllerTest(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task PostEndpoint_Create_Single_Valid()
        {
            var url = "/book";
            var appRequest = new
            {
                Client = new
                {
                    FirstName = "AppFName",
                    LastName = "AppLName",
                    Email = "AppFL@mail.com",
                    Phone = "12345678901"
                },
                TreatmentId = new[]{ "5ec8579645e16549ec3afd7e" },
                Notes = "asd",
                Date = "2020-07-17",
                StartTime = "12:00:00"
            };

            var response = await _client.PostAsync(url, ContentHelper.GetStringContent(appRequest));
            var readResponse = await response.Content.ReadAsStringAsync();
            var app = JsonConvert.DeserializeObject<Appointment>(readResponse);

            await _client.DeleteAsync($"{url}/{app.ID}");

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PostEndpoint_Create_Multiple_Valid()
        {
            var url = "/book";
            var appRequest = new
            {
                Client = new
                {
                    FirstName = "AppFName",
                    LastName = "AppLName",
                    Email = "AppFL@mail.com",
                    Phone = "09876543211"
                },
                TreatmentId = new[] { "5ec8579645e16549ec3afd7e",
                "5ec8579645e16549ec3afd7e"},
                Notes = "",
                Date = "2020-07-17",
                StartTime = "14:00:00"
            };

            var response = await _client.PostAsync(url, ContentHelper.GetStringContent(appRequest));
            var readResponse = await response.Content.ReadAsStringAsync();
            var app = JsonConvert.DeserializeObject<Appointment>(readResponse);

            await _client.DeleteAsync($"{url}/{app.ID}");


            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PostEndpoint_Create_Valid_On_FullDay()
        {
            var url = "/book";
            var appRequest = new
            {
                Client = new
                {
                    FirstName = "AppFName",
                    LastName = "AppLName",
                    Email = "AppFL@mail.com",
                    Phone = "09876543211"
                },
                TreatmentId = new[] { "5ec8579645e16549ec3afd7e",
                "5ec8579645e16549ec3afd7e"},
                Notes = "",
                Date = "2020-05-30",
                StartTime = "10:00:00"
            };

            var response = await _client.PostAsync(url, ContentHelper.GetStringContent(appRequest));


            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetEndpoint_GetByDate_Valid()
        {
            var response = await _client.GetAsync("/book/2020-05-30");
            var readResponse = await response.Content.ReadAsStringAsync();
            var app = JsonConvert.DeserializeObject<IEnumerable<Appointment>>(readResponse);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        [Fact]
        public async Task PutEndpoint_ValidApp()
        {
            var url = "/book";
            var appRequest = new AppointmentDetails
            {
                Client = new ClientDetails
                {
                    FirstName = "AppFName",
                    LastName = "AppLName",
                    Email = "AppFL@mail.com",
                    Phone = "09876543211"
                },
                TreatmentId = new List<string>{ "5ec8579645e16549ec3afd7e"},
                Notes = "",
                Date = "2020-07-14",
                StartTime = "10:00:00"
            };

            var response = await _client.PostAsync(url, ContentHelper.GetStringContent(appRequest));
            var readResponse = await response.Content.ReadAsStringAsync();
            var app = JsonConvert.DeserializeObject<Appointment>(readResponse);

            appRequest.StartTime = "11:00:00";

            var updateResponse = await _client.PutAsync($"{url}/{app.ID}", ContentHelper.GetStringContent(appRequest));
            var readUpdatedResponse = await updateResponse.Content.ReadAsStringAsync();
            var updatedApp = JsonConvert.DeserializeObject<Appointment>(readUpdatedResponse);

            await _client.DeleteAsync($"{url}/{app.ID}");

            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.True(updatedApp.Info.StartTime == appRequest.StartTime);

        }

        [Fact]
        public async Task PutEndpoint_ValidApp_EmployeeNA()
        {
            var url = "/book/5ed1a0e1ec6a11218b7420ae";
            var appRequest = new AppointmentDetails
            {
                Client = new ClientDetails
                {
                    FirstName = "AppFName",
                    LastName = "AppLName",
                    Email = "AppFL@mail.com",
                    Phone = "09876543211"
                },
                TreatmentId = new List<string>{ "5ec8579645e16549ec3afd7e",
                "5ec8579645e16549ec3afd7e"},
                Notes = "",
                Date = "2020-05-30",
                StartTime = "10:00:00"
            };

            appRequest.StartTime = "11:00:00";

            var updateResponse = await _client.PutAsync(url, ContentHelper.GetStringContent(appRequest));
            var readUpdatedResponse = await updateResponse.Content.ReadAsStringAsync();
            var updatedApp = JsonConvert.DeserializeObject<Appointment>(readUpdatedResponse);

            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }

        [Fact]
        public async Task PostEndpoint_ValidApp_ClosedDay()
        {
            var url = "/book";
            var appRequest = new AppointmentDetails
            {
                Client = new ClientDetails
                {
                    FirstName = "AppFName",
                    LastName = "AppLName",
                    Email = "AppFL@mail.com",
                    Phone = "09876543211"
                },
                TreatmentId = new List<string>{ "5ec8579645e16549ec3afd7e",
                "5ec8579645e16549ec3afd7e"},
                Notes = "",
                Date = "2020-05-31",
                StartTime = "10:00:00"
            };

            var response = await _client.PostAsync(url, ContentHelper.GetStringContent(appRequest));
            var readResponse = await response.Content.ReadAsStringAsync();
            var app = JsonConvert.DeserializeObject<string>(readResponse);

            Assert.Equal("Appointment was not created", app);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteEndpoint_ValidApp()
        {
            var url = "/book";
            var appRequest = new AppointmentDetails
            {
                Client = new ClientDetails
                {
                    FirstName = "AppFName",
                    LastName = "AppLName",
                    Email = "AppFL@mail.com",
                    Phone = "09876543211"
                },
                TreatmentId = new List<string>{ "5ec8579645e16549ec3afd7e"
                },
                Notes = "",
                Date = "2020-05-16",
                StartTime = "15:00:00"
            };

            var response = await _client.PostAsync(url, ContentHelper.GetStringContent(appRequest));
            var readResponse = await response.Content.ReadAsStringAsync();
            var app = JsonConvert.DeserializeObject<Appointment>(readResponse);

            var deleteResponse =  await _client.DeleteAsync($"{url}/{app.ID}");

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        }

        [Fact]
        public async Task DeleteEndpoint_App_That_Does_Not_Exist()
        {
            var deleteResponse = await _client.DeleteAsync("book/invalidappointmentid");
        }
    }
}
