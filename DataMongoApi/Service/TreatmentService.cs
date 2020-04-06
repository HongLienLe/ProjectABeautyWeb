using System;
using System.Collections.Generic;
using DataMongoApi.Models;
using MongoDB.Driver;

namespace DataMongoApi.Service
{
    public class TreatmentService
    {
        private readonly IMongoCollection<Treatment> _treatment;

        public TreatmentService(ISalonDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _treatment = database.GetCollection<Treatment>(settings.SalonCollectionName);
        }

        public List<Treatment> Get() =>
            _treatment.Find(treatment => true).ToList();

        public Treatment Get(string id) =>
            _treatment.Find<Treatment>(t => t.Id == id).FirstOrDefault();

        public Treatment Create(Treatment treatment)
        {
            _treatment.InsertOne(treatment);
            return treatment;
        }

        public void Update(string id, Treatment treatmentIn) =>
            _treatment.ReplaceOne(t => t.Id == id, treatmentIn);

        public void Remove(Treatment treatmentIn) =>
            _treatment.DeleteOne(t => t.Id == treatmentIn.Id);

        public void Remove(string id) =>
            _treatment.DeleteOne(t => t.Id == id);
    }
}
