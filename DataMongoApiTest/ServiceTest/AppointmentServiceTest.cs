//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using DataMongoApi.DbContext;
//using DataMongoApi.Middleware;
//using DataMongoApi.Models;
//using DataMongoApi.Service;
//using DataMongoApi.Service.InterfaceService;
//using MongoDB.Driver;
//using Moq;
//using NUnit.Framework;

//namespace DataMongoApiTest.ServiceTest
//{
//    public class AppointmentServiceTest
//    {
//        private Mock<IMongoCollection<Appointment>> _mockCollection;
//        private Mock<IEmployeeService> _employeeService;
//        private Mock<IMongoDbContext> _mockContext;
//        private Mock<IClientService> _clientService;
//        private Mock<ITreatmentService> _treatmentService;
//        private Mock<IOperatingHoursService> _ophrService;
//        private AppointmentService _appService;
//        private List<Appointment> _list;
//        private Appointment _appointment;

//        [SetUp]
//        public void SetUp()
//        {

//            _appointment = new Appointment()
//            {
//                Info = new AppointmentDetails()
//                {
//                    Client = new ClientDetails()
//                    {
//                        FirstName = "Test",
//                        LastName = "LTest",
//                        Email = "TFL@mail.com",
//                        Phone = "123"
//                    },
//                    StartTime = "10:00:00",
//                    EndTime = "10:45:00",
//                    TreatmentId = new List<string>() { "TREATMENTID" },
//                    Date = "2020-05-09"
//                },
//                EmployeeId = "EMPLOYEEID"


//            };

//            _mockCollection = new Mock<IMongoCollection<Appointment>>();
//            _mockCollection.Object.InsertOne(_appointment);
//            _mockContext = new Mock<IMongoDbContext>();
//            _list = new List<Appointment>();
//            _list.Add(_appointment);

//            Mock<IAsyncCursor<Appointment>> _appCursor = new Mock<IAsyncCursor<Appointment>>();
//            _appCursor.Setup(_ => _.Current).Returns(_list);
//            _appCursor
//                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
//                .Returns(true)
//                .Returns(false);

//            //Mock FindSync
//            _mockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Appointment>>(),
//            It.IsAny<FindOptions<Appointment, Appointment>>(),
//             It.IsAny<CancellationToken>())).Returns(_appCursor.Object);

//            //Mock GetCollection
//            _mockContext.Setup(c => c.GetCollection<Appointment>("Appointments")).Returns(_mockCollection.Object);


//            _clientService = new Mock<IClientService>();
//            _treatmentService = new Mock<ITreatmentService>();
//            _ophrService = new Mock<IOperatingHoursService>();
//            _employeeService = new Mock<IEmployeeService>();

//            _appService = new AppointmentService(_mockContext.Object, _clientService.Object, _ophrService.Object, _treatmentService.Object, _employeeService.Object);

//        }

//        [Test]
//        public void Process_Valid_Appointement()
//        {
//            var requestedApp = new AppointmentDetails()
//            {
//                Client = new ClientDetails()
//                {
//                    FirstName = "Test",
//                    LastName = "LTest",
//                    Email = "TFL@mail.com",
//                    Phone = "123"
//                },
//                StartTime = "11:00:00",
//                EndTime = "11:45:00",
//                TreatmentId = new List<string>() { "TREATMENTID" },
//                Date = "2020-05-09"

//            };

//            _treatmentService.Setup(x => x.Get(It.IsAny<string>())).Returns(
//                new Treatment()
//                {
//                    ID = "TREATMENTID",
//                    About = new TreatmentDetails()
//                    {
//                        TreatmentName = "Full Set",
//                        TreatmentType = "SNS",
//                        Duration = 45,
//                        Price = 28,
//                        isAddOn = false
//                    }
//                });
//            _ophrService.Setup(x => x.Get(It.IsAny<string>())).
//                Returns(new OperatingHours()
//                {
//                    ID = "DAYID",
//                    About = new OperatingHoursDetails()
//                    {
//                        Day = "Saturday",
//                        OpeningHr = "10:00:00",
//                        ClosingHr = "19:00:00",
//                        isOpen = true
//                    },
//                    Employees = new List<string>() { "employeeId" }

//                });

//            _employeeService.Setup(x => x.Get(It.IsAny<string>())).Returns(
//                 new Employee()
//                 {
//                     ID = "EMPLOYEEID",
//                     Details = new EmployeeDetails()
//                     {
//                         Name = "Employee1",
//                         Email = "E1@mail.com"
//                     },

//                     Treatments = new List<string>() {"TREATMENTID"},
//                     WorkDays = new List<string>() { "monday" } 
//                 });

//            _clientService.Setup(x => x.GetByContactNo(It.IsAny<string>())).Returns((Client)null);
//            _clientService.Setup(x => x.Create(It.IsAny<ClientDetails>())).Returns(new Client()
//            {
//                ID = "ClientID",
//                About = requestedApp.Client
//            });
//            _clientService.Setup(x => x.AddAppointment(It.IsAny<string>(), It.IsAny<Appointment>())).Verifiable();

//            var actual = _appService.ProcessAppointment(requestedApp);

//            Assert.IsNotNull(actual);
//        }

//        [Test]
//        public void Return_Null_If_Employee_Not_Avaliable_For_App_ProcessApp()
//        {
//            var requestedApp = new AppointmentDetails()
//            {
//                Client = new ClientDetails()
//                {
//                    FirstName = "Test",
//                    LastName = "LTest",
//                    Email = "TFL@mail.com",
//                    Phone = "123"
//                },
//                StartTime = "10:00:00",
//                EndTime = "10:45:00",
//                TreatmentId = new List<string>() { "TREATMENTID" },
//                Date = "2020-05-09"

//            };

//            _treatmentService.Setup(x => x.Get(It.IsAny<string>())).Returns(
//                new Treatment()
//                {
//                    ID = "TREATMENTID",
//                    About = new TreatmentDetails()
//                    {
//                        TreatmentName = "Full Set",
//                        TreatmentType = "SNS",
//                        Duration = 45,
//                        Price = 28,
//                        isAddOn = false
//                    }
//                });
//            _ophrService.Setup(x => x.Get(It.IsAny<string>())).
//                Returns(new OperatingHours()
//                {
//                    ID = "DAYID",
//                    About = new OperatingHoursDetails()
//                    {
//                        Day = "Saturday",
//                        OpeningHr = "10:00:00",
//                        ClosingHr = "19:00:00",
//                        isOpen = true
//                    },
//                    Employees = new List<string>() { "EMPLOYEEID" }

//                });

//            _employeeService.Setup(x => x.Get(It.IsAny<string>())).Returns(new Employee()
//            {
//                ID = "EMPLOYEEID",
//                Details = new EmployeeDetails()
//                {
//                    Name = "Employee1",
//                    Email = "E1@mail.com"
//                },

//                Treatments = new List<string>() { "TREATMENTID" },
//                WorkDays = new List<string>() { "monday" }

//            });

//            _clientService.Setup(x => x.GetByContactNo(It.IsAny<string>())).Returns((Client)null);
//            _clientService.Setup(x => x.Create(It.IsAny<ClientDetails>())).Returns(new Client()
//            {
//                ID = "ClientID",
//                About = requestedApp.Client
//            });
//            _clientService.Setup(x => x.AddAppointment(It.IsAny<string>(), It.IsAny<Appointment>())).Verifiable();
//            _clientService.Setup(x => x.Create(It.IsAny<ClientDetails>())).Returns(new Client()
//            {
//                ID = "ClientID",
//                About = requestedApp.Client
//            });
//            _clientService.Setup(x => x.AddAppointment(It.IsAny<string>(), It.IsAny<Appointment>())).Verifiable();

//            var actual = _appService.ProcessAppointment(requestedApp);

//            Assert.IsNull(actual);
//        }

//        [Test]
//        public void Return_Booked_Appointments()
//        {
//            var actualAppointments = _appService.GetAppointments("2020-05-09");

//            Assert.IsTrue(actualAppointments.Count() == 1);
//        }
//    }
//}
