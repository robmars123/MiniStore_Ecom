using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Business.Entities;
using PayPal.Api;

namespace MiniStore.Controllers
{
    public class PaymentController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Payment
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        
        
        //private APIContext GetAPIContext()
        //{
        //    // Authenticate with PayPal
        //    var config = ConfigManager.Instance.GetProperties();
        //    var accessToken = new OAuthTokenCredential(config).GetAccessToken();
        //    var apiContext = new APIContext(accessToken);
        //    return apiContext;
        //}
        
    }
}
