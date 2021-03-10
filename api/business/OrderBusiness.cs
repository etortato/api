using business.Inbound;
using business.Outbound;
using domain;
using domain.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace business
{
    public class OrderBusiness : BaseBusiness
    {
        public OrderBusiness(DatabaseContext context) : base(context) { }
        public List<OrderOutbound> GetList()
        {
            return db.OrderDishes.Select(orders =>
                new OrderOutbound
                {
                    OrderId = orders.OrderId,
                    Dish = orders.Dish.Name,
                    DishType = orders.Dish.DishType,
                    TimeOfDay = orders.Dish.TimeOfDay.Name
                })
                .ToList();
        }
        public int New()
        {
            Order newOrder = new Order();
            db.Orders.Add(newOrder);
            db.SaveChanges();
            return newOrder.OrderId;
        }
        public void Validate(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException("Order");
            if (!db.Orders.Any(a => a.OrderId == id)) throw new KeyNotFoundException("Order");
        }
        public bool HasDish(OrderDishInbound newOrderDish)
        {
            Validate(newOrderDish.OrderId);
            DishBusiness dishBusiness = new DishBusiness(db);
            dishBusiness.Validate(newOrderDish.DishId);
            return db.OrderDishes.Any(a => a.OrderId == newOrderDish.OrderId && a.DishId == newOrderDish.DishId);
        }
        public int AddDish(OrderDishInbound newOrderDish)
        {
            Validate(newOrderDish.OrderId);
            DishBusiness dishBusiness = new DishBusiness(db);
            dishBusiness.Validate(newOrderDish.DishId);
            if (!dishBusiness.CanHaveMultiple(newOrderDish.DishId) && HasDish(newOrderDish))
                throw new Exception($"Multiple orders of {dishBusiness.Get(newOrderDish.DishId).Name} is not allowed.");

            OrderDish newEntry = new OrderDish
            {
                DishId = newOrderDish.DishId,
                OrderId = newOrderDish.OrderId
            };
            db.OrderDishes.Add(newEntry);
            db.SaveChanges();

            return newEntry.OrderDishId;
        }
        #region This code should be on an Application layer but to simplify I put it here :) 
        private string[] BasicOrderValidation_FromApplicationLayer(string order)
        {
            if (order == null) throw new ArgumentNullException("order");
            if (order.Length == 0) throw new FormatException("Invalid entry. Order can not be blank.");
            string[] orderCommands = order.Split(',');
            if (orderCommands.Length == 1) throw new FormatException("Invalid entry. Order must select at least the time of the day and 1 dish.");
            if (!orderCommands[0].ToLower().Equals("morning") && !orderCommands[0].ToLower().Equals("night")) throw new Exception("Time of day must be 'morning' or 'night'.");
            return orderCommands;
        }
        private string FormatDishesNames(List<DishOutbound> dishes)
        {
            List<string> formattedDish = new List<string>();
            foreach (var dish in dishes) formattedDish.Add($"{dish.Number} - ({dish.DishType}) {dish.Name} / ");
            string result = string.Join(" / ", formattedDish.ToArray());
            return result.Remove(result.Length - 3);
        }
        private int[] OrderDishesValidation_FromApplicationLayer(string[] order, int timeOfDayId, string timeOfDay)
        {
            List<int> orderNumericCommands = new List<int>();
            for (int i = 1; i < order.Length; i++)
            {
                if (!int.TryParse(order[i], out int result)) throw new Exception($"Order dishes must be numbers. {order[i]} is not a number.");
                orderNumericCommands.Add(result);
            }
            DishBusiness dishBusiness = new DishBusiness(db);
            List<DishOutbound> validDishes = dishBusiness.GetDishesByTimeOfDay(timeOfDayId);
            foreach (var command in orderNumericCommands)
                if (!validDishes.Any(a => a.Number == command)) 
                    throw new Exception($"For {timeOfDay} orders the dishes are: {FormatDishesNames(validDishes)}");
            return orderNumericCommands.ToArray();
        }
        public List<OrderOutbound> NewOrder_FromApplicationLayer(string order)
        {
            #region Validation
            string[] orderCommands = BasicOrderValidation_FromApplicationLayer(order);
            TimeOfDayBusiness timeOfDayBusiness = new TimeOfDayBusiness(db);
            TimeOfDayOutbound time = timeOfDayBusiness.Get(orderCommands[0].ToLower());
            int[] orderNumericCommands = OrderDishesValidation_FromApplicationLayer(orderCommands, time.TimeOfDayId, orderCommands[0].ToLower());
            #endregion
            // Creates a new order
            int newOrderId = New();
            List<OrderOutbound> newOrder = new List<OrderOutbound>();
            // Adds all dishes to the new order
            DishBusiness dishBusiness = new DishBusiness(db);
            foreach (int dishNumber in orderNumericCommands)
            {
                DishOutbound dish = dishBusiness.Get(dishNumber, time.TimeOfDayId);
                AddDish(new OrderDishInbound
                {
                    DishId = dish.DishId,
                    OrderId = newOrderId
                });
                newOrder.Add(new OrderOutbound
                {
                    Dish = dish.Name,
                    DishType = dish.DishType,
                    OrderId = newOrderId,
                    TimeOfDay = time.Name
                });
            }
            return newOrder;
        }
        #endregion
    }
}
