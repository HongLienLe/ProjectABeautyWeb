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
                Body = new EmployeeForm()
                {
                    Details = new EmployeeDetails{
                    Name = "EmployeeTest",
                    Email = "ETest@mail.com"
                    },
                    Treatments = new List<string>()
                    {
                        "5eecc6790fcc0e79a1973bb9",
                        "5eecc67a0fcc0e79a1973bba",
                        "5eecc67a0fcc0e79a1973bbb",
                        "5eecc67a0fcc0e79a1973bbc",
                        "5eecc67a0fcc0e79a1973bbd",
                        "5eecc67a0fcc0e79a1973bbe",
                        "5eecc67a0fcc0e79a1973bbf",
                        "5eecc67a0fcc0e79a1973bc0",
                        "5eecc67a0fcc0e79a1973bc1",
                        "5eecc67a0fcc0e79a1973bc2"
                    },
                    WorkDays = new List<string>()
                    {
                         "monday",
                         "tuesday",
                         "wednesday",
                         "thursday",
                         "friday",
                         "saturday"
                    }  
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
        public async Task EmployeeId_In_Treatment()
        {
            var url = "/admin/treatment/5ef3b8605e3b112e5c423d34";

            var response = await _client.GetAsync(url);
            var value = await response.Content.ReadAsStringAsync();
            var treatment = JsonConvert.DeserializeObject<Treatment>(value);

            var employeeId = "5ef3b8695e3b112e5c423d48";
            Assert.Contains(employeeId, treatment.Employees);
        }

        [Fact]
        public async Task EmployeeId_In_OperatingHours()
        {
            var url = "/admin/operatinghours/monday";

            var response = await _client.GetAsync(url);
            var value = await response.Content.ReadAsStringAsync();
            var day = JsonConvert.DeserializeObject<OperatingHours>(value);


            var employeeId = new EmployeeIdName()
            {
                Id = "5ef3b8695e3b112e5c423d48"
                ,
                Name = "Michael"
            };

            Assert.Contains(employeeId, day.Employees);
        }

        [Fact]
        public async Task PostEndpoint_InvalidEmployee_ReturnUnSuccess()
        {
            var request = new
            {
                Url = "/admin/employee",
                Body = new EmployeeForm()
                {
                    Details = new EmployeeDetails()
                    {
                        Name = "E1",
                        Email = "Invalid@mail.com"
                    }
                }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("/admin/employee")]
        [InlineData("/admin/employee/5ef3b8695e3b112e5c423d48")]
        public async Task GetEndpoint_Get_Valid(string endpoint)
        {
            var response = await _client.GetAsync(endpoint);
            var stringResponse = await response.Content.ReadAsStringAsync();
            //var employees = JsonConvert.DeserializeObject<IEnumerable<Employee>>(stringResponse);
            //Assert.Contains(employees, e => e.Details.Name == "UpdatedName");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
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
                Body = new EmployeeForm()
                {
                    Details = new EmployeeDetails()
                    {
                        Name = "EmployeeTestUpdate",
                        Email = "ETest@mail.com"
                    }
                }
            };

            var newResponse = await _client.PostAsync(newRequest.Url, ContentHelper.GetStringContent(newRequest.Body));
            var newValue = await newResponse.Content.ReadAsStringAsync();
            var newEmployee = JsonConvert.DeserializeObject<Employee>(newValue);

            var request = new
            {
                Url = $"/admin/employee/{newEmployee.ID}",
                Body = new EmployeeForm()
                {
                    Details = new EmployeeDetails()
                    {
                        Name = "UpdatedName",
                        Email = "Update@mail.com"
                    }
                }
            };

            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<Employee>(value);

            await _client.DeleteAsync($"/admin/employee/{newEmployee.ID}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(request.Body.Details.Name, employee.Details.Name);
        }

        [Fact]
        public async Task PutEndpoint_Update_InvalidId()
        {
            var request = new
            {
                Url = "admin/employee/InvalidEmployeeId",
                Body = new EmployeeForm()
                {
                    Details = new EmployeeDetails()
                    {
                        Name = "X",
                        Email = "Invalid@mail.com"
                    }
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
                Body = new EmployeeForm()
                {
                    Details = new EmployeeDetails(){
                    Name = "EmployeeTestUpdateInvalid",
                    Email = "ETest@mail.com"
                    }
                }
            };

            var newresponse = await _client.PostAsync(newEmployeeRequest.Url, ContentHelper.GetStringContent(newEmployeeRequest.Body));
            var newvalue = await newresponse.Content.ReadAsStringAsync();
            var newemployee = JsonConvert.DeserializeObject<Employee>(newvalue);

            var request = new
            {
                Url = $"/admin/employee/{newemployee.ID}",
                Body = new EmployeeForm()
                {
                    Details = new EmployeeDetails()
                    {
                        Name = "invalidNumber123",
                        Email = "Update@mail.com"
                    }
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
                Body = new EmployeeForm()
                {
                    Details = new EmployeeDetails()
                    {
                        Name = "EmployeeTestDelete",
                        Email = "ETest@mail.com"
                    }
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
                Body = new EmployeeForm()
                {
                    Details = new EmployeeDetails()
                    {
                        Name = "EmployeeTestRemoveTreatment",
                        Email = "ETest@mail.com"
                    },
                    Treatments = new List<string>()
                    {
                            "5eecc6790fcc0e79a1973bb9"
                    }
                }
            };

            var newResponse = await _client.PostAsync(newRequest.Url, ContentHelper.GetStringContent(newRequest.Body));
            var newValue = await newResponse.Content.ReadAsStringAsync();
            var newEmployee = JsonConvert.DeserializeObject<Employee>(newValue);

            await _client.DeleteAsync($"/admin/employee/{newEmployee.ID}");


            var treatmentResponse = await _client.GetAsync("/admin/treatment/5eecc6790fcc0e79a1973bb9");
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
                Body = new EmployeeForm()
                {
                    Details = new EmployeeDetails()
                    {
                        Name = "EmployeeTestRemoveFromWorkdays",
                        Email = "ETest@mail.com"
                    },
                    WorkDays = new List<string>()
                    {

                        "monday"
                    }
                }
            };

            var newResponse = await _client.PostAsync(newRequest.Url, ContentHelper.GetStringContent(newRequest.Body));
            var newValue = await newResponse.Content.ReadAsStringAsync();
            var newEmployee = JsonConvert.DeserializeObject<Employee>(newValue);

            await _client.DeleteAsync($"/admin/employee/{newEmployee.ID}");


            var dayResponse = await _client.GetAsync("/admin/operatinghours/5ec8838bfa32864c1f2e5e19");
            var dayValue = await dayResponse.Content.ReadAsStringAsync();
            var day = JsonConvert.DeserializeObject<OperatingHours>(dayValue);

            var employeeId = new EmployeeIdName()
            {
                Id = newEmployee.ID
               ,
                Name = newRequest.Body.Details.Name
            };

            Assert.DoesNotContain(employeeId, day.Employees);
        }

        //Check that treatment ids are 24 string
        //work includes day and ids

        //deletes in the correct areas
    }
}
