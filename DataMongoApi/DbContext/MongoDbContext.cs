using System;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DataMongoApi.DbContext
{
    public class MongoDbContext : IMongoDbContext

    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public IClientSessionHandle Session { get; set; }

        public MongoDbContext(IOptions<SalonDatabaseSettings> settings, IClientConfiguration clientConfiguration)
        {
            _mongoClient = new MongoClient(settings.Value.ConnectionString);
            _db = _mongoClient.GetDatabase(clientConfiguration.MerchantId);

        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            return _db.GetCollection<T>(name);
        }
    }

    public interface IMongoDbContext
    {
        IMongoCollection<Book> GetCollection<Book>(string name);
    }
}