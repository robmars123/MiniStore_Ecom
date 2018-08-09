using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Business.Entities
{
    [Table("Carts")]
    public class Cart
    {
        public Cart()
        {
            this.Quantity = 1;
            this.Status = 1;
        }
        [Key]
        public int Cart_Id { get; set; }
        public int Product_Id { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Product_Description { get; set; }
        public string Product_Name { get; set; }
        [ForeignKey("Product_Id")]
        public virtual Product Product { get; set; }
        public string User_Id { get; set; }
        public int Status { get; set; }

        [NotMapped]
        public decimal? PriceTotal
        {
            get
            {
                decimal? _calculatedPrice = (Quantity > 1) ? UnitPrice * Quantity : UnitPrice;
                return _calculatedPrice;
            }
        }
    }
}