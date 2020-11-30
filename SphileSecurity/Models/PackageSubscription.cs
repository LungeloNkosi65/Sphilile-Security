using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SphileSecurity.Models
{
    public class PackageSubscription
    {
        [Key]
        public int PackageSubscriptionId { get; set; }
        public int SecurityPackageId { get; set; }
        public virtual SecurityPackage SecurityPackage { get; set; }
        [DisplayName("Customer Email")]
        public string CustomerEmail { get; set; }
        [DisplayName("Subscription Fee"),DataType(DataType.Currency)]
        public decimal SubscriptionFee { get; set; }
        [DisplayName("Status")]
        public string SubStatus { get; set; }
        [DisplayName("Subscribe Date")]
        public DateTime DateSubscribed { get; set; }
        [DisplayName("Subscription Reference")]
        public string SubReference { get; set; }


        ApplicationDbContext db = new ApplicationDbContext();
        public decimal getPackagePrice()
        {
            var price = (from package in db.SecurityPackages
                         where SecurityPackageId == package.SecurityPackageId
                         select package.PackagePrice).FirstOrDefault();
            return price;
        }
    }
}