using System;
using System.Collections.Generic;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public interface IWorkScheduleRepo
    {
        public void addWorkSchedule(WorkScheduleModel wsm);
        public List<OperatingTime> GetEmployeeWorkSchedule(int employeeId);
        public List<Employee> GetEmployeeByWorkDay(int dayId);
    }
}
