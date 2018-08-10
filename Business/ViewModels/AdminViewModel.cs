using Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Business.ViewModels
{
    public class AdminViewModel
    {
        public Order Order { get; set; }
        public Product Product { get; set; }
        public List<Order> Orders { get;set; }
        public List<Product> productList { get; set; }
        public ProductImage PrimaryProduct_Image { get; set; }
        public int OrderCount { get; set; }
        public decimal Total { get; set; }
    }
}