using Business.Data;
using Business.Entities;
using Business.Enums;
using Business.Extensions;
using Business.Generics;
using Business.Interfaces;
using Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Business.DataService
{
    public class ProductDataService : BaseContext, IProductDataService
    {
        ProductViewModel productVM = new ProductViewModel();
        //MEN - Navigation link
        public List<Product> GetProductList(int catNumber)
        {
            if (catNumber == (int)Categories.Kids) catNumber = (int)Categories.Kids;
            else if (catNumber == (int)Categories.Men) catNumber = (int)Categories.Men;
            else if (catNumber == (int)Categories.Women) catNumber = (int)Categories.Women;
            productVM.productList = ProductList.GetLists(db.Products.Where(p => p.Category_Id == catNumber).Select(s => s).ToList());
            return productVM.productList;
        }

        //Query - Get List
        public List<Product> GetProducts()
        {
            productVM.productList = ProductList.GetLists(db.Products.ToList());
            return productVM.productList;
        }

        public Product GetProductDetails(int? Id)
        {
            return ProductList.GetItem(db.Products.Find(Id));
        }

        public List<Category> GetCategories()
        {
            return ProductList.GetLists(db.Categories.ToList());
        }

        public List<Subcategory> GetSubCategories()
        {
            return ProductList.GetLists(db.Subcategories.ToList());
        }

        public List<ProductImage> GetImages(int productId)
        {
            List <ProductImage> img = db.ProductImages.Where(x => x.Product_Id == productId).Select(y => y).ToList();
            foreach (var item in img)
            {
                string imreBase64Data = Convert.ToBase64String(item.Image);
                item.ConvertedProductImage = string.Format("data:image/png;base64,{0}", imreBase64Data);
            }
            return img;
        }
        public void AddImage(ProductImage img, HttpPostedFileBase file)
        {
            if (file != null)
            {
                img.Image = new byte[file.ContentLength];
                file.InputStream.Read(img.Image, 0, file.ContentLength);
            }
            db.ProductImages.Add(img);
            db.SaveChanges();
        }

        public void AddProduct(Product product)
        {
            try
            {
                product.Date_Added = DateTime.Now;
                product.Subcategory_Id = product.Subcategory_Id;
                db.Products.Add(product);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string error = ex.InnerException.InnerException.ToString();
            }

        }

        public void UpdateProduct(Product product)
        {
            product.Date_Modified = DateTime.Now;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteProduct(int? Id)
        {
            Product product = db.Products.Find(Id);
            db.Products.Remove(product);
            db.SaveChanges();
        }

    }
}