using System;
using System.Collections.Generic;
using DataMongoApi.Models;

namespace DataMongoApi.Service.InterfaceService
{
    public interface ITreatmentService
    {
        public List<Treatment> Get();
        public Treatment Create(Treatment treatment);
        public Treatment Get(string id);
        public void Update(string id, TreatmentDetails treatmentIn);
        public void Remove(string id);
        public void UpdateEmployee(string id, List<string> employeeids);
    }
}
