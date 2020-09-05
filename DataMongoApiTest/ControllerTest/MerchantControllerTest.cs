//using System;
//using DataMongoApi.Controllers.AdminController;
//using DataMongoApi.Models;
//using DataMongoApi.Service.InterfaceService;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using NUnit.Framework;

//namespace DataMongoApiTest.ControllerTest
//{
//    public class MerchantControllerTest
//    {
//        private Mock<IMerchantService> _merchantService;
//        private MerchantController _merchantController;

//        [SetUp]
//        public void Setup()
//        {
//            _merchantService = new Mock<IMerchantService>();

//        }

//        [Test]
//        public void Get_Return_200()
//        {
//            _merchantService.Setup(x => x.Get(It.IsAny<string>())).Returns(new Merchant()
//            {
//               Name = "Salon",
//               Email = "Salon@mail.com",
//               Phone = "123"
//            });

//            _merchantController = new MerchantController(_merchantService.Object);

//            var actual = _merchantController.Get("123123") as ObjectResult;

//            Assert.AreEqual(actual.StatusCode, 200);
//        }

//        [Test]
//        public void Get_With_Invalid_Id_Return_404()
//        {
//            _merchantService.Setup(x => x.Update(It.IsAny<string>(), It.IsAny<Merchant>()));

//            _merchantController = new MerchantController(_merchantService.Object);

//            var actual = _merchantController.Get("Invalid") as ObjectResult;

//            Assert.AreEqual(actual.StatusCode, 404);

//        }

//        [Test]
//        public void Create_New_Merchant_Return_200()
//        {
//            _merchantService.Setup(x => x.Create(It.IsAny<Merchant>())).Returns(new Merchant());

//            _merchantController = new MerchantController(_merchantService.Object);

//            var actual = _merchantController.Create(new Merchant()) as ObjectResult;

//            Assert.AreEqual(actual.StatusCode, 200);
//        }

//        [Test]
//        public void Update_Existing_Merchant_Return_200()
//        {
//            _merchantService.Setup(x => x.Get(It.IsAny<string>())).Returns(new Merchant());
//            _merchantService.Setup(x => x.Update(It.IsAny<string>(), It.IsAny<Merchant>()));
                
//            _merchantController = new MerchantController(_merchantService.Object);

//            var actual = _merchantController.Update("merchantid", new Merchant()) as ObjectResult;

//            Assert.AreEqual(actual.StatusCode, 200);
//        }

//    }
//}
