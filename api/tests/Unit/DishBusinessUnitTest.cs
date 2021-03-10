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
    public class DishBusinessUnitTest : BaseUnitTest
    {
        private readonly DishBusiness service;
        public DishBusinessUnitTest()
        {
            service = new DishBusiness(db);
        }
        [Fact]
        public void ValidateById_OutOfRange()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => service.Validate(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => service.Validate(0));
        }
        [Fact]
        public void ValidateById_KeyNotFound()
        {
            Assert.Throws<KeyNotFoundException>(() => service.Validate(100));
        }
        [Fact]
        public void ValidateById_KeyFound()
        {
            service.Validate(1);
            Assert.True(true);
        }
        [Fact]
        public void ValidateByNumberTimeOfDay_OutOfRange()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => service.Validate(-1, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => service.Validate(0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => service.Validate(1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => service.Validate(1, -1));
        }
        [Fact]
        public void ValidateByNumberTimeOfDay_KeyNotFound()
        {
            Assert.Throws<KeyNotFoundException>(() => service.Validate(100, 1));
            Assert.Throws<KeyNotFoundException>(() => service.Validate(1, 100));
        }
        [Fact]
        public void ValidateByNumberTimeOfDay_KeyFound()
        {
            service.Validate(1, 1);
            Assert.True(true);
        }
        [Fact]
        public void GetById()
        {
            DishOutbound dish = service.Get(1);
            Assert.Equal(1, dish.DishId);
        }
        [Fact]
        public void GetByNumberTimeOfDay()
        {
            DishOutbound dish = service.Get(1, 1);
            Assert.Equal(1, dish.DishId);
        }
        [Fact]
        public void CanHaveMultiple_False()
        {
            int dishId = db.Dishes.Where(w => w.CanHaveMultiple == false).Select(s => s.DishId).First();
            Assert.False(service.CanHaveMultiple(dishId));
        }
        [Fact]
        public void CanHaveMultiple_True()
        {
            int dishId = db.Dishes.Where(w => w.CanHaveMultiple).Select(s => s.DishId).First();
            Assert.True(service.CanHaveMultiple(dishId));
        }
        [Fact]
        public void ValidDishNumbers()
        {
            List<DishOutbound> validDishNumbersMorning = service.GetDishesByTimeOfDay(1);
            Assert.Equal(3, validDishNumbersMorning.Count);
            List<DishOutbound> validDishNumbersNight = service.GetDishesByTimeOfDay(2);
            Assert.Equal(4, validDishNumbersNight.Count);
        }
    }
}
