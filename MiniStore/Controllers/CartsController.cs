using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Business.DataService;
using Business.Entities;
using Business.ViewModels;
using Microsoft.AspNet.Identity;

namespace MiniStore.Controllers
{
    public class CartsController : Controller
    {
        private CartDataService _cartDataService = new CartDataService();
        private CartViewModel _cartProductList = new CartViewModel();
        private static string _userID;
        // GET: Carts
        public ActionResult Index()
        {
            GetUserID(out _userID);
            _cartProductList.CartProducts = _cartDataService.GetAddedCartProducts(_userID);
            PrimaryProductImage(_cartProductList);
            return View(_cartProductList);
        }
        private void PrimaryProductImage(CartViewModel _cartProductList)
        {
            _cartProductList.CartProducts.ForEach(item =>
           _cartProductList.PrimaryProduct_Image = _cartDataService.GetImages(item.Product_Id).FirstOrDefault()
           );
        }
        public void GetUserID(out string userID)
        {
            _userID = User.Identity.GetUserId(); //logged in User
            //userID = null;
            //if no loggedIn user, grab sessionID - loggedIn_User will be null.
            //get sessionID and save as userID - unregistered users
            userID = (_userID != null) ? userID = _userID : HttpContext.Session.SessionID;
        }
        public ActionResult Add(ProductViewModel _product)
        {
            GetUserID(out _userID);
            _cartDataService.AddProductToCart(_product, _userID);
            return RedirectToAction("Index", "Carts");
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cart cart)
        {
            if (ModelState.IsValid)
            {
                //db.Carts.Add(cart);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cart);
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(FormCollection cartForm, int id)
        {
            if (ModelState.IsValid)
            {
                Cart cartItem = new Cart();
                var quantity = cartForm.GetValues("item.Quantity").FirstOrDefault();
                cartItem.Quantity = Convert.ToInt32(quantity);
                _cartDataService.Update(cartItem, id);
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Carts/Delete/5
        public ActionResult Remove(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _cartDataService.Remove(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
