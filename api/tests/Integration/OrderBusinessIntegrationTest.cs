using business;
using business.Inbound;
using business.Outbound;
using domain;
using domain.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using tests.Unit;
using Xunit;

namespace tests
{
    public class OrderBusinessIntegrationTest : BaseIntegrationTest
    {
        private readonly OrderBusiness service;
        public OrderBusinessIntegrationTest()
        {
            service = new OrderBusiness(db);
        }
        [Fact]
        public void NewOrder_NullOrder()
        {
            Assert.Throws<ArgumentNullException>(() => service.NewOrder_FromApplicationLayer(null));
        }
        [Fact]
        public void NewOrder_EmptyOrder()
        {
            Assert.Throws<FormatException>(() => service.NewOrder_FromApplicationLayer(string.Empty));
        }
        [Fact]
        public void NewOrder_RandomOrder()
        {
            Assert.Throws<FormatException>(() => service.NewOrder_FromApplicationLayer(Guid.NewGuid().ToString()));
        }
        [Fact]
        public void NewOrder_RandomOrderCommaSeparated()
        {
            Assert.Throws<Exception>(() => service.NewOrder_FromApplicationLayer(Guid.NewGuid().ToString() + "," + Guid.NewGuid().ToString()));
        }
        [Fact]
        public void NewOrder_Morning_Empty()
        {
            Assert.Throws<FormatException>(() => service.NewOrder_FromApplicationLayer("morning"));
        }
        [Fact]
        public void NewOrder_Morning_OrderEmptyValue()
        {
            Assert.Throws<Exception>(() => service.NewOrder_FromApplicationLayer("morning,"));
        }
        [Fact]
        public void NewOrder_Morning_RandomDish()
        {
            Assert.Throws<Exception>(() => service.NewOrder_FromApplicationLayer("morning," + Guid.NewGuid().ToString()));
        }
        [Fact]
        public void NewOrder_Night_RandomDish()
        {
            Assert.Throws<Exception>(() => service.NewOrder_FromApplicationLayer("night," + Guid.NewGuid().ToString()));
        }
        [Fact]
        public void NewOrder_Morning_OrdersNightDishNumber()
        {
            Assert.Throws<Exception>(() => service.NewOrder_FromApplicationLayer("morning,4"));
        }
        [Fact]
        public void NewOrder_Morning_OrdersEggs()
        {
            List<OrderOutbound> result = service.NewOrder_FromApplicationLayer("morning,1");
            Assert.Single(result);
            Assert.Equal(1, result[0].OrderId);
            Assert.Equal("eggs", result[0].Dish);
        }
        [Fact]
        public void NewOrder_Morning_OrdersTwoEggs()
        {
            Assert.Throws<Exception>(() => service.NewOrder_FromApplicationLayer("morning,1,1"));
        }
        [Fact]
        public void NewOrder_Morning_OrdersEggsAndTwoCoffees()
        {
            List<OrderOutbound> result = service.NewOrder_FromApplicationLayer("morning,1,3,3");
            Assert.Equal(3, result.Count);
            Assert.Equal(1, result[0].OrderId);
            Assert.Equal("eggs", result[0].Dish);
            Assert.Equal("coffee", result[1].Dish);
        }
    }
}
