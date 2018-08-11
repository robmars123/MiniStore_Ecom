using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Business.Enums;

namespace Business.Entities
{

    public class Order
    {
        [Key]
        [DisplayName("Order")]
        public int Order_Id { get; set; }
        public int Customer_Id { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipCountry { get; set; }
        public DateTime ShipDate { get; set; }
        public int PostalCode { get; set; }
        public string State { get; set; }
        [DisplayName("Total")]
        public decimal Order_Value { get; set; }
        public string Other_Details { get; set; }
        [DisplayName("Date")]
        public string Order_Date { get; set; }
        public DateTime Date_Modified { get; set; }
        public DateTime Date_Removed { get; set; }
        [DisplayName("Payment Status")]
        public int? Payment_Status { get; set; }

        public virtual Customer Customer { get; set; }
    }
}