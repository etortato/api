using domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace domain.model
{
    public class Dish
    {
        public int DishId { get; set; }
        public int Number { get; set; }
        [NotMapped]
        public string DishType 
        { 
            get 
            {
                return (DishTypeEnum)Number switch
                {
                    DishTypeEnum.entree => "Entreé",
                    DishTypeEnum.side => "Side",
                    DishTypeEnum.drink => "Drink",
                    DishTypeEnum.desert => "Desert",
                    _ => "Invalid"
                };
            } 
        }
        public string Name { get; set; }
        public bool CanHaveMultiple { get; set; }
        public int TimeOfDayId { get; set; }
        public TimeOfDay TimeOfDay { get; set; }
        public List<OrderDish> Orders { get; set; }
    }
}
