using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailsId { get; set; }
        public int CartId { get; set; }
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [DisplayName("User Email")]
        public string UserEmail { get; set; }
        public byte[] Picture { get; set; }
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }
        public int OrderId { get; set; }
    }
}