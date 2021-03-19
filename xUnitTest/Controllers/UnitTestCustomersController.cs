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
using FluentAssertions;
using System.Threading;

namespace xUnitTest.Controllers
{
    public class CustomersControllerTests
    {

        

        [Theory]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        public async void Create_ExistedID_ReturnViewResultCustomer(int id)
        {
            var customer = new Customer { FirstName = "RamilQ", LastName = "NaumQ"+id.ToString(), Address = "LosQ-Ang", Discount = "5", Id = id };
            var option = new DbContextOptionsBuilder<ShoppingContext>().
                UseInMemoryDatabase(databaseName: "testsCreate").Options;
            var context = new ShoppingContext(option);
            SampleData.Initialize(context);
            var mock = new Mock<CustomersService>(context);
            var controller = new CustomersController(mock.Object);
            var resultView = await controller.Create(customer);
            var viewResult = Assert.IsType<RedirectToActionResult>(resultView);
            var actionName = Assert.IsAssignableFrom<string>(viewResult.ActionName);
            
            var resultViewDetails = await controller.Details(id);
            var viewResultDetails = Assert.IsType<ViewResult>(resultViewDetails);
            var model = Assert.IsAssignableFrom<Customer>(viewResultDetails.Model);

            Assert.Equal("Index", actionName);
            Assert.Equal(model, customer);
        }
        [Theory]
        [InlineData(6)]
        [InlineData(4)]
        public async void Edit_ExistedId_ReturnViewResultCustomer(int id)
        {
            var customer = new Customer { FirstName = "RamilQ", LastName = "NaumQ", Address = "LosQ-Ang", Discount = "5", Id = id };
            var option = new DbContextOptionsBuilder<ShoppingContext>().
                UseInMemoryDatabase(databaseName: "testsEdit").Options;
            var context = new ShoppingContext(option);
            SampleData.Initialize(context);
            var mock = new Mock<CustomersService>(context);
            var controller = new CustomersController(mock.Object);
            var resultView = await controller.Edit(id, customer);
            var viewResult = Assert.IsType<RedirectToActionResult>(resultView);
            var actionName = Assert.IsAssignableFrom<string>(viewResult.ActionName);

            var resultViewDetails = await controller.Details(id);
            var viewResultDetails = Assert.IsType<ViewResult>(resultViewDetails);
            var model = Assert.IsAssignableFrom<Customer>(viewResultDetails.Model);

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
        public async void Details_ExistedId_ReturnViewResultCustomer(int a)
        {
            var option = new DbContextOptionsBuilder<ShoppingContext>().
                UseInMemoryDatabase(databaseName: "testsDBDetails").Options;
            var context = new ShoppingContext(option);
            SampleData.Initialize(context);
            var mock = new Mock<CustomersService>(context);
            var controller = new CustomersController(mock.Object);
            var resultView = await controller.Details(a);
            var viewResult = Assert.IsType<ViewResult>(resultView);
            var model = Assert.IsAssignableFrom<Customer>(viewResult.Model);
            Assert.Equal(a, model.Id);
        }
        [Theory]
        [InlineData("", SortState.LastNameAsc)]
        [InlineData("", SortState.LastNameDesc)]
        [InlineData("", SortState.AddressAsc)]
        [InlineData("", SortState.AddressDesc)]
        [InlineData("Ramil", SortState.LastNameAsc)]
        [InlineData("Bob", SortState.LastNameDesc)]
        [InlineData("Tony", SortState.AddressAsc)]
        [InlineData("Naum", SortState.AddressDesc)]


        public async void Index_ExistedSearchString_SortState_ReturnViewResultCustomer(string searchString, SortState sortParameter)
        {
            var option = new DbContextOptionsBuilder<ShoppingContext>().
                UseInMemoryDatabase(databaseName: "testsDBIndex").Options;
            var context = new ShoppingContext(option);
            SampleData.Initialize(context);
            var mock = new Mock<CustomersService>(context);
            var controller = new CustomersController(mock.Object);
            Customer[] customers = new Customer[]
            {
                new Customer {FirstName = "Ramil", LastName = "Naum", Address = "Los-Ang", Discount = "5", Id = 1},
                new Customer {FirstName = "Bob", LastName = "Dillan", Address = "Berlin", Discount = "7", Id = 2},
                new Customer {FirstName = "Kile", LastName = "Rise", Address = "London", Discount = "0", Id = 3},
                new Customer {FirstName = "John", LastName = "Konor", Address = "Vashington", Discount = "3", Id = 4},
                new Customer {FirstName = "Tony", LastName = "Stark", Address = "Florida", Discount = "5", Id = 5},
                new Customer {FirstName = "Jamie", LastName = "Lanister", Address = "Westerros", Discount = "10", Id = 6},
            };

            

            var customersSort = SortMethod(customers, sortParameter, searchString).ToList();

            var resultView = await controller.Index(searchString, sortParameter);

            var viewResult = Assert.IsType<ViewResult>(resultView);

            var modelSort = Assert.IsAssignableFrom<IEnumerable<Customer>>(viewResult.Model).ToList();

            for (int i = 0; i < customersSort.Count; i++)
            {
                Assert.Equal(customersSort[i], modelSort[i]);
            }

            //customers.Should().BeEquivalentTo(mode);

        }
        private IEnumerable<Customer> SortMethod(Customer[] customers, SortState sortOrder, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(s => s.LastName.Contains(searchString)
                                                 || s.FirstName.Contains(searchString)).ToArray();
            }
            return sortOrder switch
            {
                SortState.LastNameDesc => customers.OrderByDescending(c => c.LastName),
                SortState.AddressAsc => customers.OrderBy(c => c.Address),
                SortState.AddressDesc => customers.OrderByDescending(c => c.Address),
                _ => customers.OrderBy(c => c.LastName),
            };

        }



    }
}
