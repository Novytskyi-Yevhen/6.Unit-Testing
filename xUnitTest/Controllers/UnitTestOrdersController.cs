using System;
using Xunit;
using Moq;
using ShoppingSystem.Controllers;
using ShoppingSystem.Models;
using ShoppingSystem.Services;
using ShoppingSystem.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace xUnitTest.Controllers
{
   public class UnitTestOrdersController
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public async void TestOrdersDetails(int a)
        {
            var option = new DbContextOptionsBuilder<ShoppingContext>().UseInMemoryDatabase(databaseName: "testsDB")
                      .Options;
            var context = new ShoppingContext(option);
            SampleData.Initialize(context);

            var supermarketService = new Mock<SupermarketsService>(context);
            var customerstService = new Mock<CustomersService>(context);
            var mock = new Mock<OrdersService>(context, supermarketService.Object, customerstService.Object);

            var controller = new OrdersController(mock.Object);

            var resultView = await controller.Details(a);

            var viewResult = Assert.IsType<ViewResult>(resultView);
            var model = Assert.IsAssignableFrom<Order>(viewResult.Model);
            Assert.Equal(a, model.Id);


        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public async void Details_ExistedId_ReturnViewResultOrders(int a)
        {
            var option = new DbContextOptionsBuilder<ShoppingContext>().UseInMemoryDatabase(databaseName: "testDB4").Options;
            var context = new ShoppingContext(option);
            SampleData.Initialize(context);

            var supermarketService = new Mock<SupermarketsService>(context);
            var customerstService = new Mock<CustomersService>(context);
            var mock = new Mock<OrdersService>(context, supermarketService.Object, customerstService.Object);
            var controller = new OrdersController(mock.Object);
            var resultView = await controller.Details(a);
            var viewResult = Assert.IsType<ViewResult>(resultView);
            var model = Assert.IsAssignableFrom<Order>(viewResult.Model);
            Assert.Equal(a, model.Id);
        }

        

    }
}
