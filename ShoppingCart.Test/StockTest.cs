using FizzWare.NBuilder;
using NSubstitute;
using NUnit.Framework;
using ShoppingCart.Business.Service;
using ShoppingCart.Data.DAL.Data.Interfaces;
using ShoppingCart.Data.DML.Model;
using System;
using Microsoft.Extensions.DependencyInjection;


namespace ShoppingCart.Test
{
    [TestFixture]
    [Category("UnitTests.ShoppingCartServiceTest")]
    public class StockTest
    {

        IServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {
            _serviceProvider = Substitute.For<IServiceProvider>();
            AddProducts_HappyPath();
        }

        [Test]
        public void  FindCustomerService_HappyPath()
        {
            var CartRepository = Substitute.For<IShoppingCartRepository>();
            var request = Guid.NewGuid();
            var productResponse = Builder<CustomerShoppingCart>.CreateNew().Build();

            CartRepository.GetByIdAsync(request).Returns(productResponse);

            _serviceProvider.GetService<IShoppingCartRepository>().Returns(CartRepository);
            var productService = new ShoppingCartService(_serviceProvider);

            Assert.True(productService.Get(new Business.Model.ShoppingCartServiceRequest { Id = request , CustomerId = 14573 }) != null);
        }

        [Test]
        public void AddProducts_HappyPath ()
        {
            GenerateDummy dummyGeneration = new GenerateDummy(_serviceProvider);
            var products = dummyGeneration.GenerateDummyProductList();
            var CartRepository = Substitute.For<IShoppingCartRepository>();

            _serviceProvider.GetService<IShoppingCartRepository>().Returns(CartRepository);
            var productService = new ShoppingCartService(_serviceProvider);
            Assert.True(productService.AddProducts(products) != null);
        }
    }
}