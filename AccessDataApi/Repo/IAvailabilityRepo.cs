using System;
using System.Collections.Generic;
using AccessDataApi.Models;
using Itenso.TimePeriod;

namespace AccessDataApi.Repo
{
    public interface IAvailabilityRepo
    {
        public List<Employee> GetWorkingEmployeesByDateAndTreatment(DateTime date, int treatmentId);
        public ITimePeriodCollection GetAvailbilityByEmployee(DateTime date, Employee employee);
        public ITimePeriodCollection GetAvailableTime(DateTime date);
    }
}
