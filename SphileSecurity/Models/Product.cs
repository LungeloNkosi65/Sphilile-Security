using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string SupplierId { get; set; }
        public int CategoryId { get; set; }
        [DisplayName("Produt Name")]
        public string ProductName { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        public int Quantity { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public byte[] Image { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual Category Category { get; set; }

    }
}