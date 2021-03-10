using domain;
using domain.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tests
{
    public static class Arranges
    {
        public static void Orders_3NewOrders(DatabaseContext db)
        {
            db.Orders.AddRange(
                new Order(),
                new Order(),
                new Order()
            );
            db.SaveChanges();
        }
        public static void Orders_1NewOrder(DatabaseContext db)
        {
            db.Orders.Add(new Order());
            db.SaveChanges();
        }
        public static void TimeOfDays(DatabaseContext db)
        {
            db.TimeOfDays.AddRange(
                new TimeOfDay { TimeOfDayId = 1, Name = "morning" },
                new TimeOfDay { TimeOfDayId = 2, Name = "night" }
            );
            db.SaveChanges();
        }
        public static void Dishes(DatabaseContext db)
        {
            var morning = db.TimeOfDays.FirstOrDefault(f => f.Name == "morning");
            var night = db.TimeOfDays.FirstOrDefault(f => f.Name == "night");
            db.Dishes.AddRange(
            #region morning
                new Dish { DishId = 1, Number = 1, TimeOfDayId = 1, TimeOfDay = morning, Name = "eggs", CanHaveMultiple = false },
                new Dish { DishId = 2, Number = 2, TimeOfDayId = 1, TimeOfDay = morning, Name = "toast", CanHaveMultiple = false },
                new Dish { DishId = 3, Number = 3, TimeOfDayId = 1, TimeOfDay = morning, Name = "coffee", CanHaveMultiple = true },
            #endregion
            #region night
                new Dish { DishId = 4, Number = 1, TimeOfDayId = 2, TimeOfDay = night, Name = "steak", CanHaveMultiple = false },
                new Dish { DishId = 5, Number = 2, TimeOfDayId = 2, TimeOfDay = night, Name = "potato", CanHaveMultiple = true },
                new Dish { DishId = 6, Number = 3, TimeOfDayId = 2, TimeOfDay = night, Name = "wine", CanHaveMultiple = false },
                new Dish { DishId = 7, Number = 4, TimeOfDayId = 2, TimeOfDay = night, Name = "cake", CanHaveMultiple = false }
            #endregion
            );
            db.SaveChanges();
        }
        public static void OrderDishes_Add1DishToEachOrder(DatabaseContext db)
        {
            var dish = db.Dishes.FirstOrDefault();
            foreach (var order in db.Orders.ToList())
            {
                order.Dishes = new List<OrderDish>();
                order.Dishes.Add(
                    new OrderDish 
                    {
                        Dish = dish,
                        DishId = dish.DishId,
                        Order = order,
                        OrderId = order.OrderId
                    }
                );
            }
            db.SaveChanges();
        }
        public static void OrderDishes_AddAllMorningDishes(DatabaseContext db)
        {
            var morningDishes = db.Dishes
                .Where(w => w.TimeOfDay.Name == "morning")
                .ToList();
            foreach (var order in db.Orders.ToList())
            {
                order.Dishes = new List<OrderDish>();
                order.Dishes.AddRange(
                    morningDishes.Select(s => new OrderDish
                    {
                        Dish = s,
                        DishId = s.DishId,
                        Order = order,
                        OrderId = order.OrderId
                    })
                );
            }
            db.SaveChanges();
        }
        public static void OrderDishes_AddAllNightDishes(DatabaseContext db)
        {
            var morningDishes = db.Dishes
                .Where(w => w.TimeOfDay.Name == "night")
                .ToList();
            foreach (var order in db.Orders.ToList())
            {
                order.Dishes = new List<OrderDish>();
                order.Dishes.AddRange(
                    morningDishes.Select(s => new OrderDish
                    {
                        Dish = s,
                        DishId = s.DishId,
                        Order = order,
                        OrderId = order.OrderId
                    })
                );
            }
            db.SaveChanges();
        }
    }
}
