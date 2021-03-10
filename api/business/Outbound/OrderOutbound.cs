using System;
using System.Collections.Generic;
using System.Text;

namespace business.Outbound
{
    public class OrderOutbound
    {
        public int OrderId { get; set; }
        public string TimeOfDay { get; set; }
        public string DishType { get; set; }
        public string Dish { get; set; }
    }
}
