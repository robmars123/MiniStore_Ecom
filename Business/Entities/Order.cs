using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Business.Entities
{

    public class Order
    {
        [Key]
        public int Order_Id { get; set; }
        public int Customer_Id { get; set; }
        public int Product_Id { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipCountry { get; set; }
        public DateTime ShipDate { get; set; }
        public int PostalCode { get; set; }
        public string State { get; set; }
        public string Order_Value { get; set; }
        public string Other_Details { get; set; }
        public DateTime Order_Date { get; set; }
        public DateTime Date_Modified { get; set; }
        public DateTime Date_Removed { get; set; }
    }
}