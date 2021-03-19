using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShoppingSystem.Controllers;
using ShoppingSystem.Data;
using ShoppingSystem.Models;
using ShoppingSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace xUnitTest.Controllers
{
    public class UnitTestProductsController
    {
        [Fact]
        public async void TestProductsIndex()
        {
            var option = new DbContextOptionsBuilder<ShoppingContext>().UseInMemoryDatabase(databaseName: "testDB").Options;
            var context = new ShoppingContext(option);
            SampleData.Initialize(context);

            Product[] products = new Product[]
            {
                new Product {Name = "Asus laptop x550", Price = 880, Id = 1},
                new Product {Name = "Iphone X", Price = 1200, Id = 2},
                new Product {Name = "Samsung Galaxy X9", Price = 1100, Id = 3},
                new Product {Name = "Mouse Loditec", Price = 100, Id = 4},
                new Product {Name = "Keyboard Logitec", Price = 200, Id = 5},
                new Product {Name = "Monitor Dell", Price = 320, Id = 6},
                new Product {Name = "TV LG", Price = 1300, Id = 7},
            };
            var pro = products.ToList();
            var mock = new Mock<ProductsService>(context);
            var controller = new ProductsController(mock.Object);
            var resultView = await controller.Index();
            var viewResult = Assert.IsType<ViewResult>(resultView);


            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model).OrderBy(p => p.Id).ToList();

            for (int i = 0; i < pro.Count; i++)
            {
                Assert.Equal(products[i].Id, model[i].Id);
            }



        }





        [Theory]
        [InlineData(8)]


        public async void Create_ExistedID_ReturnViewResultProducts(int id)
        {
            var product = new Product { Name = "TV LG" + id.ToString(), Id = id };
            var option = new DbContextOptionsBuilder<ShoppingContext>().
                UseInMemoryDatabase(databaseName: "testsCreate2").Options;
            var context = new ShoppingContext(option);
            SampleData.Initialize(context);
            var mock = new Mock<ProductsService>(context);
            var controller = new ProductsController(mock.Object);
            var resultView = await controller.Create(product);
            var viewResult = Assert.IsType<RedirectToActionResult>(resultView);
            var actionName = Assert.IsAssignableFrom<string>(viewResult.ActionName);

            var resultViewDetails = await controller.Details(id);
            var viewResultDetails = Assert.IsType<ViewResult>(resultViewDetails);
            var model = Assert.IsAssignableFrom<Product>(viewResultDetails.Model);

            Assert.Equal("Index", actionName);
            Assert.Equal(model, product);
        }



        [Theory]
        [InlineData(6)]
        public async void Edit_ExistedId_ReturnViewResultProduct(int id)
        {
            var customer = new Product { Name = "TV LG" + id.ToString(), Id = id };
            var option = new DbContextOptionsBuilder<ShoppingContext>().
                UseInMemoryDatabase(databaseName: "testsEdit1").Options;
            var context = new ShoppingContext(option);
            SampleData.Initialize(context);
            var mock = new Mock<ProductsService>(context);
            var controller = new ProductsController(mock.Object);
            var resultView = await controller.Edit(id, customer);
            var viewResult = Assert.IsType<RedirectToActionResult>(resultView);
            var actionName = Assert.IsAssignableFrom<string>(viewResult.ActionName);

            var resultViewDetails = await controller.Details(id);
            var viewResultDetails = Assert.IsType<ViewResult>(resultViewDetails);
            var model = Assert.IsAssignableFrom<Product>(viewResultDetails.Model);

            Assert.Equal("Index", actionName);
            Assert.Equal(model, customer);
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
            var option = new DbContextOptionsBuilder<ShoppingContext>().
                UseInMemoryDatabase(databaseName: "testsDBDetails1").Options;
            var context = new ShoppingContext(option);
            SampleData.Initialize(context);
            var mock = new Mock<ProductsService>(context);
            var controller = new ProductsController(mock.Object);
            var resultView = await controller.Details(a);
            var viewResult = Assert.IsType<ViewResult>(resultView);
            var model = Assert.IsAssignableFrom<Product>(viewResult.Model);
            Assert.Equal(a, model.Id);
        }


        

        





    }

}

