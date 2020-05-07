using System;
using System.Collections.Generic;
using System.Threading;
using DataMongoApi.DbContext;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using DataMongoApi.Service;
using DataMongoApi.Service.InterfaceService;
using MongoDB.Driver;
using Moq;

namespace DataMongoApiTest
{
    public class AppointmentServiceTest
    {
        private Mock<IMongoCollection<Appointment>> _mockCollection;
        private Mock<IMongoDbContext> _mockContext;
        private Mock<IClientService> _clientService;
        private Mock<ITreatmentService> _treatmentService;
        private Mock<IOperatingHoursService> _ophrService;
        private AppointmentService _appService;
        private List<Appointment> _list;
        private Appointment _appointment;

        public void SetUp()
        {

            _appointment = new Appointment()
            {
                Info = new AppointmentDetails()
                {

                }
            };

            _mockCollection = new Mock<IMongoCollection<Appointment>>();
            _mockCollection.Object.InsertOne(_appointment);
            _mockContext = new Mock<IMongoDbContext>();
            _list = new List<Appointment>();
            _list.Add(_appointment);

            Mock<IAsyncCursor<Appointment>> _appCursor = new Mock<IAsyncCursor<Appointment>>();
            _appCursor.Setup(_ => _.Current).Returns(_list);
            _appCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            //Mock FindSync
            _mockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Appointment>>(),
            It.IsAny<FindOptions<Appointment, Appointment>>(),
             It.IsAny<CancellationToken>())).Returns(_appCursor.Object);

            //Mock GetCollection
            _mockContext.Setup(c => c.GetCollection<Appointment>("Appointments")).Returns(_mockCollection.Object);

            _clientService = new Mock<IClientService>();
            _treatmentService = new Mock<ITreatmentService>();
            _ophrService = new Mock<IOperatingHoursService>();
        }
    }
}
