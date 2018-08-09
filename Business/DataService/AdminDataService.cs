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
    }
}