using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class SecurityHireTypePackages
    {
        public int SecurityHireTypePackagesId { get; set; }
        public int SecurityHireTypeId { get; set; }
        public virtual SecurityHireType SecurityHireType { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Description")]
        public string  Description { get; set; }
        [DataType(DataType.Currency),Required]
        public decimal Price { get; set; }
        public int Rating { get; set; }
    }
}