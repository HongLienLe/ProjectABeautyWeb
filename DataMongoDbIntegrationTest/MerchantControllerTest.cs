//using System;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using DataMongoApi;
//using DataMongoApi.Models;
//using FluentAssertions;
//using Newtonsoft.Json;
//using Xunit;

//namespace DataMongoDbIntegrationTest
//{
//    public class MerchantControllerTest : IClassFixture<TestFixture<Startup>>
//    {
//        private HttpClient _client;
//        public MerchantControllerTest(TestFixture<Startup> fixture)
//        {
//            _client = fixture.Client;
//        }

//        [Theory]
//        [InlineData("/admin/merchant")]
//        public async Task GetEndpoint_ReadAnotherMerchantId(string endpoint)
//        {
//            var response = await _client.GetAsync($"{endpoint}");
//            var value = await response.Content.ReadAsStringAsync();
//            var details = JsonConvert.DeserializeObject<Merchant>(value);

//            response.StatusCode.Should().Be(HttpStatusCode.OK);

//        }

//        //[Fact]
//        //public async Task PostEndpoint_AddDetails()
//        //{
//        //    var request = new
//        //    {
//        //        Url = "/admin/merchant",
//        //        Body = new
//        //        {
//        //            Name = " Name",
//        //            Phone = "12345678901",
//        //            Email = "Email"
//        //        }
//        //    };

//        //    var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));


//        //    response.StatusCode.Should().Be(HttpStatusCode.OK);

//        //}

//        [Fact]
//        public async Task PutEndpoint_UpdateMerchant()
//        {

//            var updatedDetails = new
//            {
//                Name = "Updated Name",
//                Phone = "12345678901",
//                Email = "UpdatedEmail"
//            };

//            var response = await _client.PutAsync("/admin/merchant", ContentHelper.GetStringContent(updatedDetails));
//            var getMerchant = await _client.GetAsync("/admin/merchant");
//            var value = await getMerchant.Content.ReadAsStringAsync();
//            var merchant = JsonConvert.DeserializeObject<Merchant>(value);


//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//            Assert.Equal(merchant.Name, updatedDetails.Name);
//        }
//    }
//}
