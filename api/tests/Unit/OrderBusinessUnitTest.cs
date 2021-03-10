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
    public class OrderBusinessUnitTest : BaseUnitTest
    {
        private readonly OrderBusiness service;
        public OrderBusinessUnitTest()
        {
            service = new OrderBusiness(db);
        }
        [Fact]
        public void New_AddsOne()
        {
            int orderId = service.New();
            Assert.Equal(1, db.Orders.Count());
            Assert.Equal(1, orderId);
        }
        [Fact]
        public void GetList_CountIs3()
        {
            Arranges.Orders_3NewOrders(db);
            Arranges.OrderDishes_Add1DishToEachOrder(db);
            var list = service.GetList();
            Assert.Equal(3, list.Count());
        }
        [Fact]
        public void GetList_CountIs0()
        {
            var list = service.GetList();
            Assert.Empty(list);
        }
        [Fact]
        public void New_AddsMoreOne()
        {
            Arranges.Orders_3NewOrders(db);
            Arranges.OrderDishes_Add1DishToEachOrder(db);
            var list = service.GetList();
            Assert.Equal(3, list.Count());
            int orderId = service.New();
            Assert.Equal(4, db.Orders.Count());
            Assert.Equal(4, orderId);
        }
        [Fact]
        public void Validate_InvalidArgument()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => service.Validate(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => service.Validate(-1));
        }
        [Fact]
        public void Validate_KeyNotFound()
        {
            Assert.Throws<KeyNotFoundException>(() => service.Validate(1));
        }
        [Fact]
        public void Validate_KeyFound()
        {
            Arranges.Orders_1NewOrder(db);
            service.Validate(1);
            Assert.True(true);
        }
        [Fact]
        public void HasDish_OrderWithNoDish()
        {
            Arranges.Orders_1NewOrder(db);
            Assert.False(service.HasDish(new OrderDishInbound { OrderId = 1, DishId = 1 }));
        }
        [Fact]
        public void HasDish_1Order_AllMorningDishes_ContaisMorningDish()
        {
            Arranges.Orders_1NewOrder(db);
            Arranges.OrderDishes_AddAllMorningDishes(db);
            Assert.True(service.HasDish(new OrderDishInbound { OrderId = 1, DishId = 1 }));
        }
        [Fact]
        public void HasDish_1Order_AllMorningDishes_DoesNotContaisNightDish()
        {
            Arranges.Orders_1NewOrder(db);
            Arranges.OrderDishes_AddAllMorningDishes(db);
            Assert.False(service.HasDish(new OrderDishInbound { OrderId = 1, DishId = 4 }));
        }
        [Fact]
        public void AddDish_NotRepeatedDish()
        {
            Arranges.Orders_1NewOrder(db);
            int result = service.AddDish(new OrderDishInbound { OrderId = 1, DishId = 1 });
            Assert.Equal(1, result);
        }
        [Fact]
        public void AddDish_RepeatedDish_CanHaveMultiple()
        {
            Arranges.Orders_1NewOrder(db);
            Arranges.OrderDishes_AddAllMorningDishes(db);
            Dish dish = db.Dishes
                .Where(w => w.CanHaveMultiple
                         && w.TimeOfDay.Name == "morning")
                .First();
            int result = service.AddDish(new OrderDishInbound
            {
                OrderId = 1,
                DishId = dish.DishId
            });
            Assert.Equal(4, result);
        }
        [Fact]
        public void AddDish_RepeatedDish_CannotHaveMultiple()
        {
            Arranges.Orders_1NewOrder(db);
            Arranges.OrderDishes_AddAllMorningDishes(db);
            Dish dish = db.Dishes
                .Where(w => w.CanHaveMultiple == false
                         && w.TimeOfDay.Name == "morning")
                .First();
            Assert.Throws<Exception>(() => service.AddDish(new OrderDishInbound
            {
                OrderId = 1,
                DishId = dish.DishId
            }));
        }
    }
}
