using System;
using System.Collections.Generic;
using System.Linq;
using DataMongoApi.DbContext;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using DataMongoApi.Service.InterfaceService;
using Itenso.TimePeriod;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace DataMongoApi.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IMongoDbContext _context;
        private IMongoCollection<Appointment> _appointments { get; set; }
        private IEmployeeService _employeesService { get; set; }
        private IOperatingHoursService _operatingHoursService;
        private ITreatmentService _treatmentService;
        private IClientService _clientService;

        public AppointmentService(IMongoDbContext context,
            IClientService clientService,
            IOperatingHoursService operatingHoursService,
            ITreatmentService treatmentService, IEmployeeService employeeService)
        {
            _context = context;
            _appointments = _context.GetCollection<Appointment>("Appointments");
            _employeesService = employeeService;
            _operatingHoursService = operatingHoursService;
            _treatmentService = treatmentService;
            _clientService = clientService;
        }

        public Appointment ProcessAppointment(AppointmentDetails app)
        {
            var day = _operatingHoursService.Get(DateTime.Parse(app.Date).DayOfWeek.ToString());
            if (!day.About.isOpen)
                return null;

            var treatments = app.TreatmentId.Select(x => _treatmentService.Get(x));
            var treatmentTime = treatments.Select(x => x.About.Duration).Sum();
            if (treatmentTime == 0)
                return null;

            var appointment = new Appointment()
            {
                Info = app,
                ModifiedOn = DateTime.UtcNow,
                TreatmentNames = treatments.Select(x => $"{x.About.TreatmentType} {x.About.TreatmentName}").ToList(),
                TotalPrice = treatments.Select(x => x.About.Price).Sum()
            };

            app.EndTime = TimeSpan.Parse(app.StartTime).Add(TimeSpan.FromMinutes(treatmentTime)).ToString();
            var client = _clientService.GetByContactNo(app.Client.Phone) == null ? _clientService.Create(app.Client) : _clientService.GetByContactNo(app.Client.Phone);
            var employees = EmployeeWorkingIds(app.TreatmentId, DateTime.Parse(app.Date).DayOfWeek.ToString());
            var employee = FreeEmployee(employees, appointment);

            if (employee == null)
                return null;
            appointment.EmployeeId = employee.ID;
            appointment.EmployeeName = employee.Details.Name;

            var bookedApp = Create(appointment);
            _clientService.AddAppointment(client.ID, bookedApp);

            return bookedApp;
            
        }

        private Employee FreeEmployee(List<string> employees, Appointment app)
        {
            foreach (var employee in employees)
            {
                var appointmentsTimePeriods = new TimePeriodCollection();

                var appointments = _appointments.Find(x => x.EmployeeId == employee && x.Info.Date.Contains(app.Info.Date)).ToList();

                if (appointments.Count() == 0 || appointments == null)
                    return _employeesService.Get(employee);

                appointmentsTimePeriods.AddAll(appointments.Select(x => AppFunction.CastBookingToTimeRange(x)));

                var requestedAppTimeSlot = AppFunction.CastBookingToTimeRange(app);

                if (!appointmentsTimePeriods.IntersectsWith(requestedAppTimeSlot))
                    return _employeesService.Get(employee);
            }
            return null;
        }

        private Appointment Create(Appointment app)
        {
            _appointments.InsertOne(app);

            return app;
        }

        public List<ReadAppointment> GetAppointments(string date)
        {
            var app = _appointments.AsQueryable<Appointment>().Where(x => x.Info.Date.Contains(date)).ToList();
            var readApp = app.Select(x => new ReadAppointment()
            {
                ID = x.ID,
                ModifiedOn = x.ModifiedOn,
                Clients = x.Info.Client,
                EmployeeId = x.EmployeeId,
                EmployeeName = x.EmployeeName,
                Date = x.Info.Date,
                StartTime = DateTime.Parse(x.Info.Date).Add(TimeSpan.Parse(x.Info.StartTime)),
                EndTime = DateTime.Parse(x.Info.Date).Add(TimeSpan.Parse(x.Info.EndTime)),
                TreatmentId = x.Info.TreatmentId,
                TreatmentNames = x.TreatmentNames,
                MiscPrice = x.MiscPrice,
                HasBeenProcess = x.HasBeenProcess,
                Notes = x.Info.Notes
            }).ToList();

            return readApp;
        }

        public void Remove(string appointmentId)
        {
            var clientNo = _appointments.Find<Appointment>(x => x.ID == appointmentId).FirstOrDefault().Info.Client.Phone;

            _clientService.RemoveAppointment(clientNo, appointmentId);
            _appointments.DeleteOne(x => x.ID == appointmentId);
        }

        public Appointment UpdateAppointment(string id, AppointmentDetails app)
        {
            var filter = Builders<Appointment>.Filter.Eq(x => x.ID, id);

            var treatments = app.TreatmentId.Select(x => _treatmentService.Get(x));
            var treatmentTime = treatments.Select(x => x.About.Duration).Sum();
            if (treatmentTime == 0)
                return null;

            var appointment = new Appointment()
            {
                Info = app,
                ModifiedOn = DateTime.UtcNow,
                TreatmentNames = treatments.Select(x => $"{x.About.TreatmentType} {x.About.TreatmentName}").ToList()
            };

            app.EndTime = TimeSpan.Parse(app.StartTime).Add(TimeSpan.FromMinutes(treatmentTime)).ToString();
            var employees = EmployeeWorkingIds(app.TreatmentId, DateTime.Parse(app.Date).DayOfWeek.ToString());
            var updatedEmployee = FreeEmployee(employees, appointment);

            if (updatedEmployee == null)
                return null;

            var updatedEmployeeId = updatedEmployee.ID;
            var updatedEmployeeName = updatedEmployee.Details.Name;

            var updatedAppointment = Builders<Appointment>.Update
                .Set(x => x.Info, app)
                .Set(x => x.EmployeeId, updatedEmployeeId)
                .Set(x => x.EmployeeName, updatedEmployeeName)
                .CurrentDate(x => x.ModifiedOn);


            _appointments.UpdateOne(filter, updatedAppointment);

            return _appointments.Find(x => x.ID == id).FirstOrDefault(); 
        }

        private List<string> EmployeeWorkingIds(List<string> treatmentId, string date)
        {
            var workingEmployees = _operatingHoursService.Get(date).Employees;

            return workingEmployees.Where(x => _employeesService.Get(x).Treatments.Any(x => treatmentId.Contains(x))).ToList();
            //return _employees.AsQueryable<Employee>()
            //    .Where(x => x.WorkDays.Contains(dayId)
            //        && x.Treatments.Any(e => treatmentId.Contains(e)))
            //    .Select(x => x.ID)
            //    .ToList();
        }

    }
}
