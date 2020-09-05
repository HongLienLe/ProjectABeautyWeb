using System;
using System.Collections.Generic;
using DataMongoApi.Models;

namespace DataMongoApi.Service.InterfaceService
{
    public interface ITreatmentTypeService
    {
        public TreatmentType Create(TreatmentTypeEntry treatmentType);
        public TreatmentType Get(string id);
        public List<TreatmentType> Get();
        public void Delete(string id);
    }
}
