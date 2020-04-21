using System;
using System.Collections.Generic;
using System.Linq;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using DataMongoApi.Service.InterfaceService;
using Itenso.TimePeriod;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace DataMongoApi.Service
{
    public class AppointmentService
    {
        private IMongoCollection<Appointment> _appointments { get; set; }
        private IMongoCollection<Employee> _employees { get; set; }
        private IOperatingHoursService _operatingHoursService;
        private ITreatmentService _treatmentService;
        private IClientService _clientService;

        public AppointmentService(ISalonDatabaseSettings settings,IClientService clientService, IOperatingHoursService operatingHoursService, ITreatmentService treatmentService,  IClientConfiguration clientConfiguration )
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(clientConfiguration.MerchantId);
            _appointments = database.GetCollection<Appointment>("Appointments");
            _employees = database.GetCollection<Employee>("Employees");


            _operatingHoursService = operatingHoursService;
            _treatmentService = treatmentService;
            _clientService = clientService;
        }

        public Appointment ProcessAppointment(AppointmentDetails app)
        {
            var day = _operatingHoursService.Get(app.Date);
            if (!day.About.isOpen)
                return null;

            var treatmentTime = app.TreatmentId.Select(x => _treatmentService.Get(x).About.Duration).Sum(x => x);
            if (treatmentTime == 0)
                return null;

            var appointment = new Appointment()
            {
                Info = app,
                ModifiedOn = DateTime.UtcNow
            };

            app.EndTime = app.StartTime.AddMinutes(treatmentTime);

            var client = _clientService.GetByContactNo(app.Client.Phone) == null ? _clientService.Create(app.Client) : _clientService.GetByContactNo(app.Client.Phone);

            var employees = EmployeeWorkingIds(app.TreatmentId, app.Date);

            appointment.EmployeeId = FreeEmployee(employees, app);

            if (appointment.EmployeeId == null)
                return null;

            var bookedApp = Create(appointment);

            _clientService.AddAppointment(client.ID, bookedApp);

            return bookedApp;
            
        }

        private string FreeEmployee(List<string> employees, AppointmentDetails app)
        {
            foreach (var employee in employees)
            {
                var appointmentsTimePeriods = new TimePeriodCollection();

                var appointments = _appointments.Find(x => x.EmployeeId == employee && x.Info.Date == app.Date).ToList();

                if (appointments.Count() == 0)
                    return employee;

                appointmentsTimePeriods.AddAll(appointments.Select(x => AppFunction.CastBookingToTimeRange(x)));

                var requestedAppTimeSlot = new TimeRange(app.StartTime, app.EndTime);

                if (!appointmentsTimePeriods.IntersectsWith(requestedAppTimeSlot))
                    return employee;
            }

            return null;
        }


        private Appointment Create(Appointment app)
        {
            _appointments.InsertOne(app);

            return app;
        }

        public List<Appointment> GetAppointments(DateTime date)
        {
            var dateT = date.ToShortDateString();
            return _appointments.Find(x => x.Info.Date == dateT).ToList();
        }

        public void Remove(string appointmentId)
        {
            _appointments.DeleteOne(x => x.ID == appointmentId);
        }

        private List<string> EmployeeWorkingIds(List<string> treatmentId, string date)
        {
            var dayId = _operatingHoursService.Get(date).ID;
            return _employees.AsQueryable<Employee>()
                .Where(x => x.WorkDays.Contains(dayId) && x.Treatments.All(t => treatmentId.Contains(t)))
                .Select(x => x.ID)
                .ToList();
        }

       
    }
}
