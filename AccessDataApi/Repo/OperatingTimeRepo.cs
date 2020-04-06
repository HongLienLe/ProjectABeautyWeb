using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.Functions;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessDataApi.Repo
{
    public class OperatingTimeRepo : IOperatingTimeRepo
    {
        private ApplicationContext _context;
        private IDoes _does;

        public OperatingTimeRepo(ApplicationContext context, IDoes does)
        {
            _context = context;
            _does = does;
        }

        public List<OperatingTimeDetails> GetOperatingTimes()
        {
            return _context.OperatingTimes.Select(x => CastTo.OperatingTimeDetails(x)).ToList();
        }

        public OperatingTimeDetails GetOperatingTime(int id)
        {
            if (!_does.OpeningIdExist(id))
                return null;
            return CastTo.OperatingTimeDetails(_context.OperatingTimes.First(x => x.Id == id));
        }

        public string UpdateOperatingTime(int id, OperatingTime oper)
        {
            if (!_does.OpeningIdExist(id))
                return "Day Id does not exist";

            if (!isEndTimeLaterThanStartTime(oper))
                return "End time can not be earlier than start time";

            var choosenDay = _context.OperatingTimes.First(x => x.Id == id);
            choosenDay.StartTime = oper.StartTime;
            choosenDay.EndTime = oper.EndTime;
            choosenDay.isOpen = oper.isOpen;

            _context.SaveChanges();

            return $"Opening time {id} has been updated"; 
        }

        private bool isEndTimeLaterThanStartTime(OperatingTime operatingTime)
        {
            return operatingTime.EndTime > operatingTime.StartTime ? true : false;
        }
    }
}
