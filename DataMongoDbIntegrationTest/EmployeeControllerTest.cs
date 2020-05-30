using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataMongoApi.Models;
using Newtonsoft.Json;
using Xunit;
using DataMongoApi;
using System.Net;
using FluentAssertions;

namespace DataMongoDbIntegrationTest
{
    public class EmployeeControllerTest : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient _client;

        public EmployeeControllerTest(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;

        }

        [Fact]
        public async Task PostEndpoint_ValidEmployee_ReturnSuccess()
        {
            var request = new
            {
                Url = "/admin/employee",
                Body = new
                {
                    Name = "EmployeeTest",
                    Email = "ETest@mail.com"
                }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<Employee>(value);

            await _client.DeleteAsync($"/admin/employee/{employee.ID}");

            response.EnsureSuccessStatusCode();
            Assert.NotNull(employee);
        }

        [Fact]
        public async Task PostEndpoint_InvalidEmployee_ReturnUnSuccess()
        {
            var request = new
            {
                Url = "/admin/employee",
                Body = new
                {
                    Name = "E1",
                    Email = "Invalid@mail.com"
                }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<Employee>(value);

            Assert.Null(employee.ID);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("/admin/employee")]
        [InlineData("/admin/employee/5ec8848bfd83534c38e8c4de")]
        public async Task GetEndpoint_Get_Valid(string endpoint)
        {
            var response = await _client.GetAsync(endpoint);

            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            //var employees = JsonConvert.DeserializeObject<IEnumerable<Employee>>(stringResponse);
            //Assert.Contains(employees, e => e.Details.Name == "UpdatedName");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetEndpoint_InvalidId()
        {
            var response = await _client.GetAsync("/admin/employee/invalidEmployeeId");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PutEndpoint_Updated_Employee_Return_Success()
        {
            var newRequest = new
            {
                Url = "/admin/employee",
                Body = new
                {
                    Name = "EmployeeTestUpdate",
                    Email = "ETest@mail.com"
                }
            };

            var newResponse = await _client.PostAsync(newRequest.Url, ContentHelper.GetStringContent(newRequest.Body));
            var newValue = await newResponse.Content.ReadAsStringAsync();
            var newEmployee = JsonConvert.DeserializeObject<Employee>(newValue);

            var request = new
            {
                Url = $"/admin/employee/{newEmployee.ID}",
                Body = new
                {
                    Name = "UpdatedName",
                    Email = "Update@mail.com"
                }
            };

            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<Employee>(value);

            await _client.DeleteAsync($"/admin/employee/{newEmployee.ID}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(request.Body.Name, employee.Details.Name);
        }

        [Fact]
        public async Task PutEndpoint_Update_InvalidId()
        {
            var request = new
            {
                Url = "admin/employee/InvalidEmployeeId",
                Body = new
                {
                    Name = "X",
                    Email = "Invalid@mail.com"
                }
            };

            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PutEndpoint_Update_ValidId_InvalidDetails()
        {
            var newEmployeeRequest = new
            {
                Url = "/admin/employee",
                Body = new
                {
                    Name = "EmployeeTestUpdateInvalid",
                    Email = "ETest@mail.com"
                }
            };

            var newresponse = await _client.PostAsync(newEmployeeRequest.Url, ContentHelper.GetStringContent(newEmployeeRequest.Body));
            var newvalue = await newresponse.Content.ReadAsStringAsync();
            var newemployee = JsonConvert.DeserializeObject<Employee>(newvalue);

            var request = new
            {
                Url = $"/admin/employee/{newemployee.ID}",
                Body = new
                {
                    Name = "invalidNumber123",
                    Email = "Update@mail.com"
                }
            };

            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<Employee>(value);


            await _client.DeleteAsync($"/admin/employee/{newemployee.ID}");

            response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
        }

        
        [Fact]
        public async Task DeleteEndpoint_Employee_Return_Success()
        {
            var newRequest = new
            {
                Url = "/admin/employee",
                Body = new
                {
                    Name = "EmployeeTestDelete",
                    Email = "ETest@mail.com"
                }
            };

            var newResponse = await _client.PostAsync(newRequest.Url, ContentHelper.GetStringContent(newRequest.Body));
            var newValue = await newResponse.Content.ReadAsStringAsync();
            var newEmployee = JsonConvert.DeserializeObject<Employee>(newValue);


            var response = await _client.DeleteAsync($"/admin/employee/{newEmployee.ID}");
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteEndpoint_InvalidId()
        {
            var response = await _client.DeleteAsync("/admin/employee/invalidemployeeid");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Has_EmployeeId_Been_Removed_FromTreatment()
        {
            var newRequest = new
            {
                Url = "/admin/employee",
                Body = new
                {
                    Name = "EmployeeTestRemoveTreatment",
                    Email = "ETest@mail.com"
                }
            };

            var newResponse = await _client.PostAsync(newRequest.Url, ContentHelper.GetStringContent(newRequest.Body));
            var newValue = await newResponse.Content.ReadAsStringAsync();
            var newEmployee = JsonConvert.DeserializeObject<Employee>(newValue);


            var requestTreatment = new
            {
                Url = $"/admin/employee/{newEmployee.ID}/manage/treatment",
                Body = new[] { "5ec8579645e16549ec3afd7e" }

            };

            await _client.PostAsync(requestTreatment.Url, ContentHelper.GetStringContent(requestTreatment.Body));

            await _client.DeleteAsync($"/admin/employee/{newEmployee.ID}");


            var treatmentResponse = await _client.GetAsync("/admin/treatment/5ec8579645e16549ec3afd7e");
            var treatmentValue = await treatmentResponse.Content.ReadAsStringAsync();
            var treatment = JsonConvert.DeserializeObject<Treatment>(treatmentValue);

            Assert.DoesNotContain(newEmployee.ID, treatment.Employees);

        }

        [Fact]
        public async Task Has_EmployeeId_Been_Removed_FromWorkDays()
        {
            var newRequest = new
            {
                Url = "/admin/employee",
                Body = new
                {
                    Name = "EmployeeTestRemoveFromWorkdays",
                    Email = "ETest@mail.com"
                }
            };

            var newResponse = await _client.PostAsync(newRequest.Url, ContentHelper.GetStringContent(newRequest.Body));
            var newValue = await newResponse.Content.ReadAsStringAsync();
            var newEmployee = JsonConvert.DeserializeObject<Employee>(newValue);


            var requestWorkDay = new
            {
                Url = $"/admin/employee/{newEmployee.ID}/manage/workdays",
                Body = new[] { "5ec8838bfa32864c1f2e5e19" }

            };

            await _client.PostAsync(requestWorkDay.Url, ContentHelper.GetStringContent(requestWorkDay.Body));

            await _client.DeleteAsync($"/admin/employee/{newEmployee.ID}");


            var dayResponse = await _client.GetAsync("/admin/operatinghours/5ec8838bfa32864c1f2e5e19");
            var dayValue = await dayResponse.Content.ReadAsStringAsync();
            var day = JsonConvert.DeserializeObject<OperatingHours>(dayValue);

            Assert.DoesNotContain(newEmployee.ID, day.Employees);

        }

        [Fact]
        public async Task PostEndpoint_UpdateTreatment_Return_Success()
        {
            var request = new
            {
                Url = "/admin/employee/5ec8848bfd83534c38e8c4de/manage/treatment",
                Body = new[] { "5ec8579645e16549ec3afd7e" }

            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            var treatmentResponse = await _client.GetAsync("/admin/treatment/5ec8579645e16549ec3afd7e");
            var treatmentValue = await treatmentResponse.Content.ReadAsStringAsync();
            var treatment = JsonConvert.DeserializeObject<Treatment>(treatmentValue);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            Assert.Contains("5ec8848bfd83534c38e8c4de", treatment.Employees);
        }

        [Fact]
        public async Task PostEndpoint_UpdateWorkdays_Return_Success()
        {
            var request = new
            {
                Url = "/admin/employee/5ec8848bfd83534c38e8c4de/manage/workdays",
                Body = new[] {
                    "5ec8838bfa32864c1f2e5e19",
                    "5ec8845003d7e24c2b997c16",
                    "5ec8848e54b3e34c392900c8",
                    "5ec969c33de2bd5095082b98",
                    "5ecaecfb541d4c5e2395e9b6",
                    "5ecd444deab8e770dfab68c1"

                }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            var dayResponse = await _client.GetAsync("/admin/operatinghours/Monday");
            var dayValue = await dayResponse.Content.ReadAsStringAsync();
            var day = JsonConvert.DeserializeObject<OperatingHours>(dayValue);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.EnsureSuccessStatusCode();
            Assert.Contains("5ec8848bfd83534c38e8c4de", day.Employees);
        }

        [Fact]
        public async Task PostEndpoint_UpdateTreatment_InvalidEmployee()
        {
            var request = new
            {
                Url = "/admin/employee/InvalidEmployeeId/manage/treatment",
                Body = new[] { "5ec8579645e16549ec3afd7e" }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            var treatmentResponse = await _client.GetAsync("/admin/treatment/5ec8579645e16549ec3afd7e");
            var treatmentValue = await treatmentResponse.Content.ReadAsStringAsync();
            var treatment = JsonConvert.DeserializeObject<Treatment>(treatmentValue);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostEndpoint_InvalidTreatment_ValidEmployee()
        {
            var request = new
            {
                Url = "/admin/employee/5ec8848bfd83534c38e8c4de/manage/treatment",
                Body = new[] { "InvalidTreatmentId" }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostEndpoint_InvalidTreatment_InvalidEmployee()
        {
            var request = new
            {
                Url = "/admin/employee/InvalidEmployeeId/manage/treatment",
                Body = new[] { "InvalidTreatmentId" }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostEndpoint_UpdateWorkdays_InvalidEmployee_ValidDay()
        {
            var request = new
            {
                Url = "/admin/employee/InvalidEmployeeId/manage/workdays",
                Body = new[] { "tuesday" }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostEndpoint_UpdateWorkDay_ValidEmployee_InvalidDay()
        {
            var request = new
            {
                Url = "/admin/employee/5ec8848bfd83534c38e8c4de/manage/workdays",
                Body = new[] { "invalidworkday" }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostEndpoint_UpdateWorkDay_InvalidEmployee_InvalidDay()
        {
            var request = new
            {
                Url = "/admin/employee/invalidemployeeid/manage/workdays",
                Body = new[] { "invalidworkday" }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
