using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class AssignShift
    {
        [Key]
        public int AssignId { get; set; }
        public string EmployeeId { get; set; }
        public virtual Employees Employees { get; set; }
        public int ShiftId { get; set; }
        public virtual Shift Shift { get; set; }
    }
}