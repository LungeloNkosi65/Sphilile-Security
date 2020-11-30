using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class PackageType
    {
        [Key]
        public int PackageTypeId { get; set; }
        [DisplayName("Package Name"),Required]
        public string PackageName { get; set; }
        [DisplayName("Package Description"),Required]
        public string PackageDescription { get; set; }
    }
}