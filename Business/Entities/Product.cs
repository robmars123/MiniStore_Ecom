using Business.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Entities
{
    public partial class Product
    {
        public Product()
        {
            QuantityPerUnit = 1;
        }
        [Key]
        public int Product_Id { get; set; }

        [StringLength(50)]
        [DisplayName("Product")]
        public string Product_Name { get; set; }
        [DataType(DataType.Currency)]
        public decimal? Price { get; set; }
        [DisplayName("Type")]
        public int? Category_Id { get; set; }
        [DisplayName("Subcategory")]
        public int? Subcategory_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_Added { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_Modified { get; set; }

        public string Description { get; set; }
        [DisplayName("Quantity Per Unit")]
        public int? QuantityPerUnit { get; set; }
  

        [Column(TypeName = "date")]
        public DateTime? Date_Removed { get; set; }
        [ForeignKey("Product_Id")]
        public virtual List<ProductImage> productImages { get; set; }
        public virtual Category Categories { get; set; }
        public virtual Subcategory Subcategories { get; set; }
    }
}
