using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Business.Entities;
using PayPal.Api;
using static MiniStore.Controllers.PaymentController;

namespace MiniStore.Controllers
{
    public class PaypalController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Paypal
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }
    }
}
