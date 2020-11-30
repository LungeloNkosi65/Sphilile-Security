using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class LeaveApplication
    {
        [Key]
        public int LeaveApplicationId { get; set; }
        public string EmployeeEmail { get; set; }
        [DisplayName("From Date"), Required, DataType(DataType.Date)]
        public DateTime FromDate { get; set; }
        [DisplayName("To Date"), Required, DataType(DataType.Date)]
        public DateTime ToDate { get; set; }
        [DisplayName("Date Applied"), DataType(DataType.Date)]
        public DateTime DateApplied { get; set; }
        [DisplayName("Status")]
        public string LeaveApplicationStatus { get; set; }
    }
}