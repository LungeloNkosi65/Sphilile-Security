using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class EmployeeCheckIn
    {
        [Key]
        public int CheckInId { get; set; }
        public string ShiftName { get; set; }
        public string EmployeeEmail { get; set; }
        [DisplayName("Time In"),DataType(DataType.Time)]
        public DateTime TimeIn { get; set; }
        [DisplayName("Time Out"), DataType(DataType.Time)]
        public DateTime TimeOut { get; set; }
        [DisplayName("Date"),DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}