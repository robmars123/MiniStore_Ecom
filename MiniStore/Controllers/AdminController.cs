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
        private CustomerDataService _customerDataService = new CustomerDataService();
        private AdminViewModel _adminViewModel = new AdminViewModel();
        private CustomerViewModel _customerViewModel = new CustomerViewModel();
        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }
        public ActionResult Customers()
        {
            _customerViewModel.CustomerList = CustomerDataService.GetCustomers();
            return View(_customerViewModel);
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
            _adminViewModel.productList = _adminDataService.GetProducts();
            //Get product primary image
            PrimaryProductImage(_adminViewModel);
            return View(_adminViewModel);
        }
        //For Details Page Only - loop through each product and get their primary image
        private void PrimaryProductImage(AdminViewModel _productList)
        {
            _productList.productList.ForEach(item =>
           _productList.PrimaryProduct_Image = _adminDataService.GetImages(item.Product_Id).FirstOrDefault()
           );
        }
    }
}