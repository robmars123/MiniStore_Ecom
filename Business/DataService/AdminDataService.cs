using Business.Data;
using Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Business.DataService
{
    public class AdminDataService : BaseContext
    {
        public List<Order> GetOrders()
        {
            return db.Orders.ToList();
        }
        public decimal GetTotal()
        {
            return db.Orders.ToList().Sum(x => Convert.ToDecimal(x.Order_Value));
        }
        public List<Product> GetProducts()
        {
            return db.Products.ToList();
        }
        public List<ProductImage> GetImages(int productId)
        {
            List<ProductImage> img = db.ProductImages.Where(x => x.Product_Id == productId).Select(y => y).ToList();
            foreach (var item in img)
            {
                string imreBase64Data = Convert.ToBase64String(item.Image);
                item.ConvertedProductImage = string.Format("data:image/png;base64,{0}", imreBase64Data);
            }
            return img;
        }
    }
}