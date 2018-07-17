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
using Business.Interfaces;
using Business.ViewModels;

namespace MiniStore.Controllers
{
    public class ProductsController : Controller
    {
        private ProductDataService _productDataSerice = new ProductDataService();
        private ProductViewModel _productList = new ProductViewModel();

        // GET: Products
        public ActionResult Index()
        {
            //Get products
            _productList.productList = _productDataSerice.GetProducts();
            //Get product primary image
            PrimaryProductImage(_productList);
            return View(_productList);
        }
        public ActionResult Men()
        {
            _productList.productList = _productDataSerice.Men();
            PrimaryProductImage(_productList);
            return View(_productList);
        }
        public ActionResult Women()
        {
            _productList.productList = _productDataSerice.Women();
            PrimaryProductImage(_productList);
            return View(_productList);
        }
        //For Details Page Only - loop through each product and get their primary image
        private void PrimaryProductImage(ProductViewModel _productList)
        {
            _productList.productList.ForEach(item =>
           _productList.PrimaryProduct_Image = _productDataSerice.GetImages(item.Product_Id).FirstOrDefault()
           );
        }
        public ActionResult Kids()
        {
            _productList.productList = _productDataSerice.Kids();
            PrimaryProductImage(_productList);
            return View(_productList);
        }
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            _productList.Product = _productDataSerice.GetProductDetails(id);
            _productList.PrimaryProduct_Image = _productDataSerice.GetImages((int)id).FirstOrDefault();
            _productList.Product.productImages = _productDataSerice.GetImages((int)id);
            Session["ProductSession"] = _productList;
            return View(_productList);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.SubCategories = new SelectList(_productDataSerice.GetSubCategories(), "Subcategory_Id", "Subcategory_Name");
            ViewBag.Categories = new SelectList(_productDataSerice.GetCategories(), "Category_Id", "Category_Name", "Category_Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productDataSerice.AddProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            var product = _productDataSerice.GetProductDetails(id);

            ViewBag.SubCategories = new SelectList(_productDataSerice.GetSubCategories(), "Subcategory_Id", "Subcategory_Name", product.Subcategory_Id);
            ViewBag.Categories = new SelectList(_productDataSerice.GetCategories(), "Category_Id", "Category_Name", "Category_Id");
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, ProductImage img, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                string imageCode = "";
                if(file != null) FileUpload(img, file);
                _productDataSerice.UpdateProduct(product);
                _productDataSerice.GetImages(product.Product_Id);

                ViewBag.SubCategories = new SelectList(_productDataSerice.GetSubCategories(), "Subcategory_Id", "Subcategory_Name", product.Subcategory_Id);
                ViewBag.Categories = new SelectList(_productDataSerice.GetCategories(), "Category_Id", "Category_Name", product.Category_Id);
                return RedirectToAction("Details", "Products", new { id = product.Product_Id });
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            Product product = _productDataSerice.GetProductDetails(id);
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _productDataSerice.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    db.Dispose();
            //}
            base.Dispose(disposing);
        }

        public void FileUpload(ProductImage img, HttpPostedFileBase file)
        {
            try
            {
                _productDataSerice.AddImage(img, file);
            }
            catch (Exception e)
            {
                string error = e.Message;
            }
        }
    }
}
