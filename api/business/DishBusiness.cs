using business.Outbound;
using domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace business
{
    public class DishBusiness : BaseBusiness
    {
        public DishBusiness(DatabaseContext context) : base(context) { }
        public DishOutbound Get(int id)
        {
            Validate(id);
            return db.Dishes
                .Where(w => w.DishId == id)
                .Select(s => new DishOutbound
                {
                    DishId = s.DishId,
                    DishType = s.DishType,
                    Name = s.Name
                })
                .First();
        }
        public DishOutbound Get(int number, int timeOfDayId)
        {
            Validate(number, timeOfDayId);
            return db.Dishes
                .Where(w => w.Number == number && w.TimeOfDayId == timeOfDayId)
                .Select(s => new DishOutbound
                {
                    DishId = s.DishId,
                    DishType = s.DishType,
                    Name = s.Name
                })
                .First();
        }
        public List<DishOutbound> GetDishesByTimeOfDay(int timeOfDayId)
        {
            new TimeOfDayBusiness(db).Validate(timeOfDayId);
            return db.Dishes
                .Where(w => w.TimeOfDayId == timeOfDayId)
                .Select(s => new DishOutbound
                {
                    DishId = s.DishId,
                    DishType = s.DishType,
                    Name = s.Name,
                    Number = s.Number
                })
                .OrderBy(o => o.Number)
                .ToList();
        }
        public void Validate(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException("Dish");
            if (!db.Dishes.Any(a => a.DishId == id)) throw new KeyNotFoundException("Dish");
        }
        public void Validate(int number, int timeOfDayId)
        {
            if (number <= 0) throw new ArgumentOutOfRangeException("DishNumber");
            new TimeOfDayBusiness(db).Validate(timeOfDayId);
            if (!db.Dishes.Any(a => a.Number == number && a.TimeOfDayId == timeOfDayId)) throw new KeyNotFoundException("Dish");
        }
        public bool CanHaveMultiple(int id)
        {
            Validate(id);
            return db.Dishes.Where(w => w.DishId == id).Select(s => s.CanHaveMultiple).FirstOrDefault();
        }
    }
}
