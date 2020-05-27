using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DataMongoApi;
using DataMongoApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit;

namespace DataMongoApiTest.IntergrationTest
{
    public class IntergrationTest 
    {
        private readonly HttpClient _client;

        public IntergrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            _client = appFactory.CreateClient();
        }

        [Fact]
        public async Task Test1()
        {
            var response = await _client.GetAsync("/admin/employee");

            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var employees = JsonConvert.DeserializeObject<IEnumerable<Employee>>(stringResponse);
            Assert.Contains(employees, e => e.Details.Name == "Hong");
        }
    }
}

//public IntergrationTest()
//{
//    var config = new ConfigurationBuilder()

//    .AddJsonFile("appsettings.json")
//    .Build();


//    var connString = config.GetConnectionString("db");
//    this.clientDb = new ClientConfiguration() { MerchantId = $"test_db_{Guid.NewGuid()}" };
//    this.DbContextSettings = Options.Create(new SalonDatabaseSettings()
//    {
//        ConnectionString = connString
//    });
//    this.DbContext = new MongoDbContext(this.DbContextSettings, this.clientDb);
//}

//public IOptions<SalonDatabaseSettings> DbContextSettings { get; }
//public IClientConfiguration clientDb { get; }
//public MongoDbContext DbContext { get; }

//public void Dispose()
//{
//    var client = new MongoClient(this.DbContextSettings.Value.ConnectionString);
//    client.DropDatabase(this.clientDb.MerchantId);
//}

