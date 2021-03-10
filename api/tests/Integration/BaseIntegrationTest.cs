using domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace tests.Unit
{
    public class BaseIntegrationTest
    {
        public readonly DatabaseContext db;
        public BaseIntegrationTest()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            db = new DatabaseContext(options);
            Arranges.TimeOfDays(db);
            Arranges.Dishes(db);
        }
    }
}
