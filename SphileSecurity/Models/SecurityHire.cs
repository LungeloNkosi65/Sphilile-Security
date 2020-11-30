using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class SecurityHire
    {
        [Key]
        public int SecurityHireId { get; set; }
        public int SecurityHireTypePackagesId { get; set; }
        public virtual SecurityHireTypePackages SecurityHireTypePackages { get; set; }
        [DisplayName("Customer Email")]
        public string UserEmail { get; set; }
        [DisplayName("Date Requested"), DataType(DataType.Date)]
        public DateTime DateHired { get; set; }
        [DisplayName("Date For"),DataType(DataType.Date),Required]
        public DateTime DateFor { get; set; }
        [DataType(DataType.Currency)]
        public decimal HirePrice { get; set; }
        public string Status { get; set; }
    }
}