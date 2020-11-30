using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class Shift
    {
        [Key]
        public int ShiftId { get; set; }
        public int ShiftTypeId { get; set; }
        public virtual ShiftType ShiftType { get; set; }
        [DisplayName("Shift Name"),Required]
        public string ShiftName { get; set; }
        [DisplayName("Shift Hours")]
        public int ShiftHours { get; set; }
        [DisplayName("Time In"),DataType(DataType.Time),Required]
        public DateTime TimeIn { get; set; }
        [DisplayName("Time Out"), DataType(DataType.Time), Required]

        public DateTime TimeOut { get; set; }
        [DisplayName("Pay Rate"),DataType(DataType.Currency)]
        public decimal PayRatePerHour { get; set; }

    }
}