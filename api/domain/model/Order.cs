using System;
using System.Collections.Generic;

namespace domain.model
{
    public class Order
    {
        public int OrderId { get; set; }
        public List<OrderDish> Dishes { get; set; }
    }
}
