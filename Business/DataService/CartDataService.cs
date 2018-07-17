using Business.Data;
using Business.Entities;
using Business.Enums;
using Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Business.DataService
{
    public class CartDataService : BaseContext
    {
        CartViewModel cartVM = new CartViewModel();
        private Cart cart = new Cart();

        public List<Cart> GetAddedCartProducts(string _userID)
        {
            cartVM.CartProducts = db.Carts.Where(x=>x.Status == (int)Status.Active && x.User_Id == _userID).ToList();
            return cartVM.CartProducts;
        }

        public void AddProductToCart(ProductViewModel _product, string userID)
        {
            var existNumber = db.Carts.Where(x => x.Product_Id == _product.Product.Product_Id && x.Status == (int)Status.Active
                                && x.User_Id == userID).Count();
            if(existNumber == 0)
            {
                cart.Product_Description = _product.Product.Description;
                cart.Product_Id = _product.Product.Product_Id;
                cart.Product_Name = _product.Product.Product_Name;
                cart.UnitPrice = (decimal)_product.Product.Price;
                cart.Quantity = (int)_product.Product.QuantityPerUnit;
                if(cart.User_Id == null)
                {
                    cart.User_Id = userID;
                }
                db.Carts.Add(cart);
                db.SaveChanges();
            }
            else
            {
                //Executed when item is readded to cart
                var currentItem = db.Carts.Where(x => x.Product_Id == _product.Product.Product_Id && x.Status == (int)Status.Active
                                   && x.User_Id == userID).FirstOrDefault();
                if (currentItem.User_Id == null)
                {
                    currentItem.User_Id = userID;
                }
                currentItem.Quantity += (int)_product.Product.QuantityPerUnit;
                db.Entry(currentItem).State = EntityState.Modified;
                db.SaveChanges();
            }
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
        public void Update(Cart cart, int id)
        {
            Cart cartItem = db.Carts.Find(id);
            cartItem.Quantity = cart.Quantity;
            db.Entry(cartItem).State = EntityState.Modified;
            db.SaveChanges();

        }
        public void Remove(int id)
        {
            Cart cart = db.Carts.Find(id);
            cart.Status = (int)Status.Removed;
            db.Entry(cart).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}