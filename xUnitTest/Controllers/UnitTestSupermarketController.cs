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
   public class UnitTestSupermarketController
    {
        [Fact]
        public async void TestSupermarketsIndex()
        {
            var option = new DbContextOptionsBuilder<ShoppingContext>().UseInMemoryDatabase(databaseName: "testDB").Options;
            var context = new ShoppingContext(option);
            SampleData.Initialize(context);

            Supermarket[] supermarkets = new Supermarket[]
            {
                new Supermarket {Name = "Rozetka", Address = "Petrovka district", Id = 1},
                new Supermarket {Name = "Comfy", Address = "Obolonsky district", Id = 2 },
                new Supermarket {Name = "Foxtrot", Address = "Pechercka district", Id = 3},
                new Supermarket {Name = "Allo", Address = "Shevchenka district", Id = 4},
                new Supermarket {Name = "Citrus", Address = "Obolon, Drea Town", Id = 5},
                new Supermarket {Name = "Moyo", Address = "Skymall Troeshina", Id = 6},
                new Supermarket {Name = "Stilus", Address = "Svyatosino", Id = 7},
            };
            var supermarketsList = supermarkets.ToList();
            var mock = new Mock<SupermarketsService>(context);
            var controller = new SupermarketsController(mock.Object);
            var resultView = await controller.Index();
            var viewResult = Assert.IsType<ViewResult>(resultView);


            var model = Assert.IsAssignableFrom<IEnumerable<Supermarket>>(viewResult.Model).OrderBy(p => p.Id).ToList();

            for (int i = 0; i < supermarketsList.Count; i++)
            {
                Assert.Equal(supermarkets[i].Id, model[i].Id);
            }
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public async void Details_ExistedId_ReturnViewResultProduct(int a)
        {
            var option = new DbContextOptionsBuilder<ShoppingContext>().UseInMemoryDatabase(databaseName: "testDB").Options;
            var context = new ShoppingContext(option);
            SampleData.Initialize(context);

            var mock = new Mock<SupermarketsService>(context);
            var controller = new SupermarketsController(mock.Object);
            var resultView = await controller.Details(a);
            var viewResult = Assert.IsType<ViewResult>(resultView);
            var model = Assert.IsAssignableFrom<Supermarket>(viewResult.Model);
            Assert.Equal(a, model.Id);
        }

        [Theory]
        [InlineData(8)]


        public async void Create_ExistedID_ReturnViewResultSupermarket(int id)
        {
            var product = new Supermarket { Name = "Rozetka2", Address = "Petrovka district2", Id=id };
            var option = new DbContextOptionsBuilder<ShoppingContext>().
                UseInMemoryDatabase(databaseName: "testsCreate3").Options;
            var context = new ShoppingContext(option);
            SampleData.Initialize(context);
            var mock = new Mock<SupermarketsService>(context);
            var controller = new SupermarketsController(mock.Object);
            var resultView = await controller.Create(product);
            var viewResult = Assert.IsType<RedirectToActionResult>(resultView);
            var actionName = Assert.IsAssignableFrom<string>(viewResult.ActionName);

            var resultViewDetails = await controller.Details(id);
            var viewResultDetails = Assert.IsType<ViewResult>(resultViewDetails);
            var model = Assert.IsAssignableFrom<Supermarket>(viewResultDetails.Model);

            Assert.Equal("Index", actionName);
            Assert.Equal(model, product);
        }



        [Theory]
        [InlineData(5)]
        public async void Edit_ExistedId_ReturnViewResultProduct(int id)
        {
            var customer = new Supermarket { Name = "Rozetka2", Address = "Petrovka district2",Id=id };
            var option = new DbContextOptionsBuilder<ShoppingContext>().
                UseInMemoryDatabase(databaseName: "testsEdit3").Options;
            var context = new ShoppingContext(option);
            SampleData.Initialize(context);
            var mock = new Mock<SupermarketsService>(context);
            var controller = new SupermarketsController(mock.Object);
            var resultView = await controller.Edit(id, customer);
            var viewResult = Assert.IsType<RedirectToActionResult>(resultView);
            var actionName = Assert.IsAssignableFrom<string>(viewResult.ActionName);

            var resultViewDetails = await controller.Details(id);
            var viewResultDetails = Assert.IsType<ViewResult>(resultViewDetails);
            var model = Assert.IsAssignableFrom<Supermarket>(viewResultDetails.Model);

            Assert.Equal("Index", actionName);
            Assert.Equal(model, customer);
        }

    }
}
