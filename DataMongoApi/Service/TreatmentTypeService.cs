using System;
using System.Collections.Generic;
using DataMongoApi.DbContext;
using DataMongoApi.Models;
using DataMongoApi.Service.InterfaceService;
using MongoDB.Driver;

namespace DataMongoApi.Service
{
    public class TreatmentTypeService : ITreatmentTypeService
    {
        public IMongoCollection<TreatmentType> _treatmentType;

        public TreatmentTypeService(IMongoDbContext context)
        {
            _treatmentType = context.GetCollection<TreatmentType>("TreatmentTypes");
        }

        public TreatmentType Create(TreatmentTypeEntry treatmentType)
        {
            var tType = _treatmentType.Find(x => x.Type == treatmentType.Type).FirstOrDefault();

            if (tType != null)
                return null;

            var type = new TreatmentType()
            {
                Type = treatmentType.Type
            };
            _treatmentType.InsertOne(type);

            return type;
        }

        public TreatmentType Get(string id)
        {
           return  _treatmentType.Find<TreatmentType>(x => x.ID == id).FirstOrDefault();
        }

        public List<TreatmentType> Get()
        {
            return _treatmentType.Find(TreatmentType => true).ToList();
        }

        public void Delete(string id)
        {
            _treatmentType.DeleteOne(x => x.ID == id);
        }
    }
}
