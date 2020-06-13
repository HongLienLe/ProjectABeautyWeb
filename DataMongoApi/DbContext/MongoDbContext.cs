using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private Dictionary<string, List<string>> _databasesAndCollections;

        public MongoDbContext(IOptions<SalonDatabaseSettings> settings, IClientConfiguration clientConf)
        {
            _mongoClient = new MongoClient(settings.Value.ConnectionString);
            _db = _mongoClient.GetDatabase(clientConf.MerchantId);
        }

        public async Task<Dictionary<string, List<string>>> GetDatabasesAndCollections()
        {
            if (_databasesAndCollections != null) return _databasesAndCollections;

            _databasesAndCollections = new Dictionary<string, List<string>>();
            var databasesResult = _mongoClient.ListDatabaseNames();

            await databasesResult.ForEachAsync(async databaseName =>
            {
                var collectionNames = new List<string>();
                var database = _mongoClient.GetDatabase(databaseName);
                var collectionNamesResult = database.ListCollectionNames();
                await collectionNamesResult.ForEachAsync(
                    collectionName => { collectionNames.Add(collectionName); });
                _databasesAndCollections.Add(databaseName, collectionNames);
            });

            return _databasesAndCollections;
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
        IMongoCollection<T> GetCollection<T>(string name);
    }
}