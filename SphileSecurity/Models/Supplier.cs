using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }
        [DisplayName("Supplier Name"),Required]
        public string SupplierName { get; set; }
        [DisplayName("Supplier Email"), Required,EmailAddress]

        public string SupplierEmail { get; set; }
        [DisplayName("Contact Number"), Required,Phone]

        public string ContactNumber { get; set; }
    }
}