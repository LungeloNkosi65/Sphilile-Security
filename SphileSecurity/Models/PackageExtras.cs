using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class PackageExtras
    {
        [Key]
        public int PackageExtraId { get; set; }
        [DisplayName("Extra Name")]
        public string ExtraName { get; set; }
    }
}