using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class ShiftType
    {
        [Key]
        public int ShiftTypeId { get; set; }
        [DisplayName("Shift Type Name")]
        public string ShiftTypeName { get; set; }
    }
}