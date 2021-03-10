using domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace business
{
    public class BaseBusiness
    {
        public readonly DatabaseContext db = null;
        public BaseBusiness(DatabaseContext context) => db = context;
    }
}
