using System;
using System.Collections.Generic;
using System.Text;

namespace domain.model
{
    public class OrderDishes
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int DishId { get; set; }
        public Dish Dish { get; set; }
    }
}
