using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Business.Entities
{
    public class OrderDetail
    {
        [Key]
        public int Order_Id { get; set; }
        public int Product_Id { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
    }
}