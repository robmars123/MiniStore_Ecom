using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Business.Entities
{
    public class Brand
    {
        [Key]
        public int Brand_Id { get; set; }
        public string Brand_Name { get; set; }
        public string Brand_Image { get; set; }
    }
}