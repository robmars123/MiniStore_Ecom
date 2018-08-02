using Business.Entities;
using Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Business.Interfaces
{
    public interface IProductDataService
    {
        List<Product> GetProductList(int catNumber);
        List<Product> GetProducts();
        Product GetProductDetails(int? Id);
        List<Category> GetCategories();
        List<Subcategory> GetSubCategories();
        void AddImage(ProductImage img, HttpPostedFileBase file);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int? Id);
    }
}
