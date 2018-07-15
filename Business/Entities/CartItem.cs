using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Business.Entities
{
    public class CartItem
    {
        [Key]
        public int CartItem_Id { get; set; }
        public int Customer_Id { get; set; }
        public int Cart_Id { get; set; }
        public int Product_Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}