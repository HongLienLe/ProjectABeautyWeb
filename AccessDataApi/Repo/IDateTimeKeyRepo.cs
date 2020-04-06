using System;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public interface IDateTimeKeyRepo
    {
        public DateTimeKey GetDateTimeKey(DateTime dateTime);
    }
}
