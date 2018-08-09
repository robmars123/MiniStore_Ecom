using Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Business.ViewModels
{
    public class CartViewModel
    {
        public Cart Cart { get; set; }
        public ProductImage PrimaryProduct_Image { get; set; }
        public List<Cart> CartProducts { get; set; }

        public decimal? CalculatedPrice { get; set; }
    }
}