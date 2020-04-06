using System;
using System.Collections.Generic;
using DataMongoApi.Models;
using MongoDB.Driver;

namespace DataMongoApi.Service
{
    public class OperatingHoursService
    {
        private readonly IMongoCollection<OperatingHours> _operatinghrs;

        public OperatingHoursService(ISalonDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _operatinghrs = database.GetCollection<OperatingHours>(settings.SalonCollectionName);
        }

        public List<OperatingHours> Get() =>
            _operatinghrs.Find(operatingHours => true).ToList();

        public OperatingHours Get(string day) =>
                    _operatinghrs.Find<OperatingHours>(op => op.Day == day).FirstOrDefault();

        public OperatingHours Create(OperatingHours ophrs)
        {
            _operatinghrs.InsertOne(ophrs);
            return ophrs;
        }
        
        public void Update(string day, OperatingHours ophrsIn) =>
            _operatinghrs.ReplaceOne(op => op.Day == day, ophrsIn);

        public void Remove(OperatingHours ophrsIn) =>
            _operatinghrs.DeleteOne(op => op.Day == ophrsIn.Day);

        public void Remove(string day) =>
            _operatinghrs.DeleteOne(op => op.Day == day);
    }
}
