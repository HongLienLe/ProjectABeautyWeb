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
    public class PaymentControllerTest : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient _client;

        public PaymentControllerTest(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Post_ProcessPayment_Success()
        {
            var request = new
            {
                Url = "/admin/payment",
                Body = new OrderEntry()
                {
                    ClientId = "5ef3b8645e3b112e5c423d3e",
                    AppointmentID = "5ef3c59b62c70d2f0560c319",
                    TreatmentIds = new List<string>()
                    {
                        "5ef3b8605e3b112e5c423d34"
                    },
                    MiscPrice = 0
                }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();
            var order = JsonConvert.DeserializeObject<OrderDetails>(value);

            await _client.DeleteAsync($"{request.Url}/{order.ID}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ProcessPayment_PresentInClientHistory_Success()
        {
            var request = new
            {
                Url = "/admin/payment",
                Body = new OrderEntry()
                {
                    ClientId = "5ef3b8645e3b112e5c423d3e",
                    AppointmentID = "5ef36a2e1cb1a7237b412ede",
                    TreatmentIds = new List<string>()
                    {
                        "5ef3b8605e3b112e5c423d34"
                    },
                    MiscPrice = 0
                }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetSinglePayment_Success()
        {
            var url = "/admin/payment/5ef3c4e71a163c2ef9d7deac";

            var response = await _client.GetAsync(url);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public async Task UpdateSinglePaymentChangesMadeInHistoryAndClients()
        {

        }

        public async Task GetPayment_ForSingleMonth()
        {

        }

        public async Task GetPayments_ForSingleDay()
        {

        }

        //try to process a treatment that does not exist

        // update order with a treatment that does not exist 
    }
}
