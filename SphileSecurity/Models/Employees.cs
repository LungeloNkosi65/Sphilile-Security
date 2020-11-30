using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class Employees
    {
        [Key]
        public string EmployeeId { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [EmailAddress,DisplayName("Employee Email")]
        public string Email { get; set; }
        [DisplayName("Phone Number"), Phone]

        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        [DisplayName("Employee Type")]
        public string EmployeeType { get; set; }
        [DisplayName("Date Registerd")]
        public DateTime DateRegisterd { get; set; }
    }
}