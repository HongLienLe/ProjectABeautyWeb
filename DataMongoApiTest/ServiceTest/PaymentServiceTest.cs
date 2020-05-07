using System;
using System.Collections.Generic;
using System.Threading;
using DataMongoApi.DbContext;
using DataMongoApi.Models;
using DataMongoApi.Service;
using DataMongoApi.Service.InterfaceService;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;

namespace DataMongoApiTest.ServiceTest
{
    public class PaymentServiceTest
    {
        private Mock<IMongoCollection<OrderDetails>> _mockCollection;
        private Mock<IMongoDbContext> _mockContext;
        private PaymentService _paymentService;
        private Mock<ITreatmentService> _treatmentService;
        private List<OrderDetails> _list;
        private OrderDetails _order;

        [SetUp]
        public void SetUp()
        {
            _order = new OrderDetails()
            {
                ClientPhone = "123",
                Treatments = new List<TreatmentOrder>()
                {
                    new TreatmentOrder()
                    {
                        TreatmentId = "T1",
                        Price = 1
                    }
                },
                Total = 1
            };

            _mockCollection = new Mock<IMongoCollection<OrderDetails>>();
            _mockCollection.Object.InsertOne(_order);
            _mockContext = new Mock<IMongoDbContext>();
            _list = new List<OrderDetails>();
            _list.Add(_order);

            Mock<IAsyncCursor<OrderDetails>> _orderCursor = new Mock<IAsyncCursor<OrderDetails>>();
            _orderCursor.Setup(_ => _.Current).Returns(_list);
            _orderCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            //Mock FindSync
            _mockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<OrderDetails>>(),
            It.IsAny<FindOptions<OrderDetails, OrderDetails>>(),
             It.IsAny<CancellationToken>())).Returns(_orderCursor.Object);

            //Mock GetCollection
            _mockContext.Setup(c => c.GetCollection<OrderDetails>("Orders")).Returns(_mockCollection.Object);

        }

        [Test]
        public void Process_Appointment_Valid()
        {
            var app = new Appointment()
            {
                ID = "1",
                Info = new AppointmentDetails()
                {
                    TreatmentId = new List<string>() { "1" },
                    Client = new ClientDetails()
                    {
                        FirstName = "Test",
                        LastName = "Test",
                        Email = "Test@gmail.com",
                        Phone = "123"
                    },
                }
            };

            _treatmentService = new Mock<ITreatmentService>();
            _treatmentService.Setup(x => x.Get(It.IsAny<string>())).Returns( new Treatment()
            {
                About = new TreatmentDetails()
                {
                    TreatmentName = "Full Set",
                    TreatmentType = "SNS",
                    Duration = 45,
                    Price = 28
                }
            });

            _paymentService = new PaymentService(_mockContext.Object, _treatmentService.Object);

            var actual = _paymentService.ProcessAppointment(app);

            Assert.IsNotNull(actual);

        }

    }
}
