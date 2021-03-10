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
    public class TimeOfDayBusinessUnitTest : BaseUnitTest
    {
        private readonly TimeOfDayBusiness service;
        public TimeOfDayBusinessUnitTest()
        {
            service = new TimeOfDayBusiness(db);
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
        public void ValidateByName_KeyNotFound()
        {
            Assert.Throws<KeyNotFoundException>(() => service.Validate("afternoon"));
        }
        [Fact]
        public void ValidateByName_KeyFound()
        {
            service.Validate("morning");
            Assert.True(true);
        }
        [Fact]
        public void Get()
        {
            TimeOfDayOutbound timeOfDay = service.Get("night");
            Assert.Equal(2, timeOfDay.TimeOfDayId);
        }
    }
}
