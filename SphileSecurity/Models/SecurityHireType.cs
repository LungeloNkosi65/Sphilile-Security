using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class SecurityHireType
    {
        [Key]
        public int SecurityHireTypeId { get; set; }
        [DisplayName("Hire Type Name")]
        public string SecurityHireTypeName { get; set; }
    }
}