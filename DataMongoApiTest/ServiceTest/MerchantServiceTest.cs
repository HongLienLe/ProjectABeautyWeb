//using System;
//using System.Collections.Generic;
//using System.Threading;
//using DataMongoApi.DbContext;
//using DataMongoApi.Middleware;
//using DataMongoApi.Models;
//using DataMongoApi.Service;
//using Microsoft.Extensions.Options;
//using MongoDB.Driver;
//using Moq;
//using NUnit.Framework;

//namespace DataMongoApiTest.ServiceTest
//{
//    public class MerchantServiceTest
//    {
//        private Mock<IMongoCollection<Merchant>> _mockCollection;
//        private Mock<IMongoDbContext> _mockContext;
//        private MerchantService _merchantService;
//        private List<Merchant> _list;
//        private Merchant _merchant;

//        [SetUp]
//        public void SetUp()
//        {

//            _merchant = new Merchant()
//            {
//                Name = "Salon",
//                Email = "Salon@mail.com",
//                Phone = "123"
//            };

//            _mockCollection = new Mock<IMongoCollection<Merchant>>();
//            _mockCollection.Object.InsertOne(_merchant);
//            _mockContext = new Mock<IMongoDbContext>();
//            _list = new List<Merchant>();
//            _list.Add(_merchant);

//            Mock<IAsyncCursor<Merchant>> _merchantCursor = new Mock<IAsyncCursor<Merchant>>();
//            _merchantCursor.Setup(_ => _.Current).Returns(_list);
//            _merchantCursor
//                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
//                .Returns(true)
//                .Returns(false);

//            //Mock FindSync
//            _mockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Merchant>>(),
//            It.IsAny<FindOptions<Merchant, Merchant>>(),
//             It.IsAny<CancellationToken>())).Returns(_merchantCursor.Object);

//            //Mock GetCollection
//            _mockContext.Setup(c => c.GetCollection<Merchant>("Merchant")).Returns(_mockCollection.Object);

//            _merchantService = new MerchantService(_mockContext.Object);

//        }

//        //[Test]
//        //public void Get_Valid_Success()
//        //{
//        //    var result = _merchantService.Get();

//        //    foreach (var treatment in result)
//        //    {
//        //        Assert.NotNull(treatment);
//        //        Assert.AreEqual(treatment.Name, _merchant.Name);
//        //        break;
//        //    }

//        //    _mockCollection.Verify(c => c.FindSync(It.IsAny<FilterDefinition<Merchant>>(),
//        //        It.IsAny<FindOptions<Merchant>>(),
//        //         It.IsAny<CancellationToken>()), Times.Once);
//        //}

//        [Test]
//        public void Create_Valid_Success()
//        {
//            var merchant = new Merchant()
//            {
//                Name = "Salon",
//                Email = "Salon@mail.com",
//                Phone = "123"
//            };

//            var result = _merchantService.Create(merchant);

//            Assert.NotNull(result);
//            Assert.AreEqual(result.Name, _merchant.Name);
//            Assert.IsNotEmpty(result.ID);

//        }

//        [Test]
//        public void Read_Merchant_Valid_Success()
//        {
//            var result = _merchantService.Get(_merchant.ID);

//            Assert.NotNull(result);
//            Assert.AreEqual(result.Name, _merchant.Name);
//            Assert.IsNotEmpty(result.ID);

//        }

//        //[Test]
//        //public void Remove_Merchant_Valid_Success()
//        //{
//        //    var merchant = new Merchant()
//        //    {
//        //        Name = "Salon",
//        //        Email = "Salon@mail.com",
//        //        Phone = "123"
//        //    };

//        //    var result = _merchantService.Create(merchant);
//        //    _merchantService.Remove(result.ID);

//        //    Assert.IsTrue(_merchantService.Get().Count == 1);
//        //}

//        //[Test]
//        //public void Invalid_Id_Return_Null()
//        //{
//        //    var result = _merchantService.Get("Invalid");
//        //    Console.WriteLine(result.Name);
//        //    Assert.IsNull(result);
//        //}
//    }
//}
