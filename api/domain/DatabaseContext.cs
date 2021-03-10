using domain.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace domain
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<TimeOfDay> TimeOfDays { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<OrderDish> OrderDishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasKey(key => key.OrderId);

            modelBuilder.Entity<OrderDish>().HasKey(key => key.OrderDishId);
            modelBuilder.Entity<OrderDish>()
                .HasOne(od => od.Order)
                .WithMany(o => o.Dishes)
                .HasForeignKey(od => od.OrderId);
            modelBuilder.Entity<OrderDish>()
                .HasOne(od => od.Dish)
                .WithMany(d => d.Orders)
                .HasForeignKey(od => od.DishId);

            modelBuilder.Entity<TimeOfDay>().HasKey(key => key.TimeOfDayId);
            modelBuilder.Entity<TimeOfDay>().HasData(
                new TimeOfDay { TimeOfDayId = 1, Name = "morning" },
                new TimeOfDay { TimeOfDayId = 2, Name = "night" }
            );

            modelBuilder.Entity<Dish>().HasKey(key => key.DishId);
            modelBuilder.Entity<Dish>().HasData(
            #region morning
                new Dish { DishId = 1, Number = 1, TimeOfDayId = 1, Name = "eggs", CanHaveMultiple = false },
                new Dish { DishId = 2, Number = 2, TimeOfDayId = 1, Name = "toast", CanHaveMultiple = false },
                new Dish { DishId = 3, Number = 3, TimeOfDayId = 1, Name = "coffee", CanHaveMultiple = true },
            #endregion
            #region night
                new Dish { DishId = 4, Number = 1, TimeOfDayId = 2, Name = "steak", CanHaveMultiple = false },
                new Dish { DishId = 5, Number = 2, TimeOfDayId = 2, Name = "potato", CanHaveMultiple = true },
                new Dish { DishId = 6, Number = 3, TimeOfDayId = 2, Name = "wine", CanHaveMultiple = false },
                new Dish { DishId = 7, Number = 4, TimeOfDayId = 2, Name = "cake", CanHaveMultiple = false }
            #endregion
            );
        }
    }
}
