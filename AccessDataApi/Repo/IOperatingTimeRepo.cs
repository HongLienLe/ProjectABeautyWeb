using System;
using System.Collections.Generic;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public interface IOperatingTimeRepo
    {
        public List<OperatingTimeDetails> GetOperatingTimes();
        public OperatingTimeDetails GetOperatingTime(int id);
        public string UpdateOperatingTime(int id, OperatingTime oper);

    }
}
