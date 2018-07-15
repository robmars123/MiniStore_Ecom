using Business.Data;
using Business.Entities;
using Business.Enums;
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
        public List<Product> Men()
        {
             productVM.productList = db.Products.Where(p => p.Category_Id == (int)Categories.Men).Select(s => s).ToList();
            return productVM.productList;
        }

        //Women - Navigation link
        public List<Product> Women()
        {
            productVM.productList = db.Products.Where(p => p.Category_Id == (int)Categories.Women).Select(s => s).ToList();
            return productVM.productList;
        }

        public List<Product> Kids()
        {
            productVM.productList = db.Products.Where(p => p.Category_Id == (int)Categories.Kids).Select(s => s).ToList();
            return productVM.productList;
        }

        //Query - Get List
        public List<Product> GetProducts()
        {
            productVM.productList = db.Products.ToList();
            return productVM.productList;
        }

        public Product GetProductDetails(int? Id)
        {
            Product product = db.Products.Find(Id);
            return product;
        }

        public List<Category> GetCategories()
        {
            List<Category> categories = db.Categories.ToList();
            return categories;
        }

        public List<Subcategory> GetSubCategories()
        {
            List<Subcategory> SubCategories = db.Subcategories.ToList();
            return SubCategories;
        }

        public List<ProductImage> GetImages(int productId)
        {
            List<ProductImage> img = db.ProductImages.Where(x => x.Product_Id == productId).Select(y=>y).ToList();
            foreach(var item in img)
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
            catch(Exception ex)
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