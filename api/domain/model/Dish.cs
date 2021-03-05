using System;
using System.Collections.Generic;
using System.Text;

namespace domain.model
{
    public class Dish
    {
        public int DishId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public bool CanHaveMultiple { get; set; }
        public int TimeOfDayId { get; set; }
        public TimeOfDay TimeOfDay { get; set; }
    }
}
