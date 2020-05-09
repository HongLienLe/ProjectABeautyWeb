using System;
using System.Collections.Generic;
using DataMongoApi.Models;

namespace DataMongoApi.Service.InterfaceService
{
    public interface IOperatingHoursService
    {
        public List<OperatingHours> Get();
        public OperatingHours Create(OperatingHours ophrs);
        public void Update(string id, OperatingHoursDetails ophrsIn);
        public void Remove(OperatingHours ophrsIn);
        public void Remove(string id);
        public OperatingHours Get(string day);
    }
}
