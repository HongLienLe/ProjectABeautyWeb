using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.HTTPModels;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public class WorkScheduleRepo : IWorkScheduleRepo
    {
        private ApplicationContext _context;
        public WorkScheduleRepo(ApplicationContext context)
        {
            _context = context;
        }

        public void addWorkSchedule(WorkScheduleModel wsm)
        {
            if (wsm.isEmployee)
            {
                var workSch = _context.workSchedules.Where(x => x.EmployeeId == wsm.Id).Select(x => x.OperatingTimeId);

                var addWorkDays = wsm.Ids.Except(workSch);

                foreach (var dayId in addWorkDays)
                {
                    OperatingTimeEmployee ot = new OperatingTimeEmployee()
                    {
                        EmployeeId = wsm.Id,
                        OperatingTimeId = dayId
                    };

                    _context.workSchedules.Add(ot);
                }
                _context.SaveChanges();

                return;
            }

            var employees = _context.workSchedules.Where(x => x.OperatingTimeId == wsm.Id).Select(x => x.EmployeeId);

            var addEmployees = wsm.Ids.Except(employees);

            foreach (var employeeId in addEmployees)
            {
                OperatingTimeEmployee ot = new OperatingTimeEmployee()
                {
                    EmployeeId = employeeId,
                    OperatingTimeId = wsm.Id
                };

                _context.workSchedules.Add(ot);
            }
            _context.SaveChanges();

        }

        public List<OperatingTime> GetEmployeeWorkSchedule(int employeeId)
        {
            if (!_context.Employees.Any(x => x.EmployeeId == employeeId))
                return null;

            var employeeWorkSchedule = _context.workSchedules.Where(x => x.EmployeeId == employeeId).Select(x => x.OperatingTime).ToList();

            return employeeWorkSchedule;
        }

        public List<Employee> GetEmployeeByWorkDay(int dayId)
        {
            if (!_context.OperatingTimes.Any(x => x.Id == dayId))
                return null;

            var employeesWorkingOnChoosenDay = _context.workSchedules.Where(x => x.OperatingTimeId == dayId).Select(x => x.Employee).ToList();

            return employeesWorkingOnChoosenDay;
        }

        public bool doesEmployeeExist(int employeeId)
        {
            return _context.Employees.Any(x => x.EmployeeId == employeeId);
        }
    }
}
