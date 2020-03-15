using System;
using System.Collections.Generic;
using AccessDataApi.Models;
using Itenso.TimePeriod;

namespace AccessDataApi.Repo
{
    public interface IAvailabilityRepo
    {
        public List<DateTime> GetAvailableTime(DateTime date);
        public List<DateTime> GetAvailableTimeWithTreatment(DateTime date, int treatmentId);
    }
}
