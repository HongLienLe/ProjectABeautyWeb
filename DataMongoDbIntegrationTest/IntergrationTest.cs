using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataMongoApi;
using DataMongoApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace DataMongoDbIntegrationTest
{
    public class IntergrationTest
    {
        public readonly HttpClient _client;

        public IntergrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>();
                
               _client = appFactory.CreateClient();
        }
    }
}
