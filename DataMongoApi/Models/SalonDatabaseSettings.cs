using System;
namespace DataMongoApi.Models
{
    public class SalonDatabaseSettings : ISalonDatabaseSettings
    {
        public string SalonCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ISalonDatabaseSettings
    {
        string SalonCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
