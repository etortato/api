using business.Outbound;
using domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace business
{
    public class TimeOfDayBusiness : BaseBusiness
    {
        public TimeOfDayBusiness(DatabaseContext context) : base(context) { }
        public TimeOfDayOutbound Get(string name)
        {
            Validate(name);
            return db.TimeOfDays
                .Where(w => w.Name == name)
                .Select(s => new TimeOfDayOutbound
                {
                    TimeOfDayId = s.TimeOfDayId,
                    Name = s.Name
                })
                .First();
        }
        public void Validate(int timeOfDayId)
        {
            if (timeOfDayId <= 0) throw new ArgumentOutOfRangeException("TimeOfDay");
            if (!db.TimeOfDays.Any(a => a.TimeOfDayId == timeOfDayId)) throw new KeyNotFoundException("TimeOfDay");
        }
        public void Validate(string name)
        {
            if (!db.TimeOfDays.Any(a => a.Name == name)) throw new KeyNotFoundException("TimeOfDay");
        }
    }
}
