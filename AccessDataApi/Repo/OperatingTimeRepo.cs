using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessDataApi.Repo
{
    public class OperatingTimeRepo
    {
        private ApplicationContext _context;

        public OperatingTimeRepo(ApplicationContext context)
        {
            _context = context;
        }

        public List<OperatingTime> GetOperatingTimes()
        {
            return _context.OperatingTimes.ToList();
        }

        public OperatingTime GetOperatingTime(int id)
        {
            if (!_context.OperatingTimes.Any(x => x.Id == id))
                return null;
            return _context.OperatingTimes.First(x => x.Id == id);
        }

        public string UpdateOperatingTime(int id, OperatingTime oper)
        {
            if (!_context.OperatingTimes.Any(x => x.Id == id))
                return "Day Id does not exist";

            if (!isEndTimeLaterThanStartTime(oper))
                return "end time can not be later than start time";

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
