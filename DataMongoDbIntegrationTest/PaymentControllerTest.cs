using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DataMongoApi;
using DataMongoApi.Models;
using FluentAssertions;
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

        public async Task Post_ProcessPayment_Success()
        {
            var request = new
            {
                Url = "/admin/payment",
                Body = new OrderDetails()
                {
                    ClientId = "ClientID",
                    Treatments = new List<TreatmentOrder>()
                    {
                        new TreatmentOrder()
                        {

                        }
                    },
                    MiscPrice = 0,
                    Total = 0

                }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public async Task ProcessPayment_PresentInClientHistory_Success()
        {
            var request = new
            {
                Url = "/admin/payment",
                Body = new OrderDetails()
                {
                    ClientId = "ClientID",
                    Treatments = new List<TreatmentOrder>()
                    {
                        new TreatmentOrder()
                        {

                        }
                    },
                    MiscPrice = 0,
                    Total = 0

                }
            };

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public async Task GetSinglePayment_Success()
        {

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
    }
}
 