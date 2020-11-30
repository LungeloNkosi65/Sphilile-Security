using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }
        [DisplayName("Order Date"),DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        public string Address { get; set; }
        [DisplayName("Order Description")]
        public string OrderDescription { get; set; }
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }
    }


    public class ProductOrder
    {

    }
}