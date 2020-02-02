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
            return _context.OperatingTimes.First(x => x.Id == id);
        }

        public void UpdateOperatingTime(int id, OperatingTime oper)
        {
            using (var context = _context)
            {
                var choosenDay = context.OperatingTimes.First(x => x.Id == id);

                choosenDay.StartTime = oper.StartTime;
                choosenDay.EndTime = oper.EndTime;

                context.SaveChanges();
            }
        }
    }
}
