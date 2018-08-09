using Business.DataService;
using Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiniStore.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private AdminDataService _adminDataService = new AdminDataService();
        private AdminViewModel _adminViewModel = new AdminViewModel();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            _adminViewModel.OrderCount = _adminDataService.GetOrders().Count();
            _adminViewModel.Total = _adminDataService.GetTotal();
            return View(_adminViewModel);
        }
        public ActionResult Orders()
        {
            _adminViewModel.Orders = _adminDataService.GetOrders();
            return View(_adminViewModel);
        }
        public ActionResult Products()
        {
            _adminViewModel.Products = _adminDataService.GetProducts();
            return View(_adminViewModel);
        }
    }
}