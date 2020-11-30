using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class SubscriptionDiscount
    {
        [Key]
        public int SubscriptionDiscountId { get; set; }
        public int PackageTypeId { get; set; }
        public virtual PackageType PackageType { get; set; }
        [DisplayName("Discount Percent"), Required]
        public decimal DiscountPercent { get; set; }
        [DisplayName("Number Of Orders"), Required]
        public int NumberOfOrders { get; set; }
    }
}