using System;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.Functions;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public class DateTimeKeyRepo : IDateTimeKeyRepo
    {
        private ApplicationContext _context;
        private IDoes _does;

        public DateTimeKeyRepo(ApplicationContext context, IDoes does)
        {
            _context = context;
            _does = does;
        }

        public DateTimeKey GetDateTimeKey(DateTime dateTime)
        {
            if (!_does.DateTimeKeyExist(dateTime))
                return null;

            return _context.DateTimeKeys.Single(x => x.date == dateTime);
        }
    }
}
