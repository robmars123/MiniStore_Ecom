using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Business.Entities
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        public int Cart_Id { get; set; }
        public int Product_Id { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Product_Description { get; set; }
        public string Product_Name { get; set; }
    }
}