using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class SecurityPackage
    {
        [Key]
        public int SecurityPackageId { get; set; }
        public int PackageTypeId {get; set;}
        public virtual PackageType PackageType {get; set;}
        [DisplayName("Package Name"),Required]
        public string PackageName { get; set; }
        [DisplayName("Package Price"),DataType(DataType.Currency)]
        public decimal PackagePrice { get; set; }
        [DisplayName("Tax Percentage")]
        public decimal TaxPercent { get; set; }
        [DisplayName("Package Description")]
        public string PackageDescription { get; set; }
    }
}