using business;
using business.Outbound;
using domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Route("api/Orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DatabaseContext db;
        private readonly OrderBusiness business;
        public OrdersController(DatabaseContext context)
        {
            db = context;
            business = new OrderBusiness(context);
        }
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(business.GetList());
        }
        [HttpPost]
        public ActionResult Post([FromForm] string order)
        {
            return Ok(business.NewOrder_FromApplicationLayer(order));
        }
    }
}
