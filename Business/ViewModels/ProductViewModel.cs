using Business.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Business.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public List<Product> productList { get; set; }
        public ProductImage PrimaryProduct_Image { get; set; }
        public List<ProductImage> productImagesList { get; set; }

        public virtual ProductImage productImage { get; set; }

        public virtual Category Categories { get; set; }

        public virtual Subcategory Subcategories { get; set; }
    }
}