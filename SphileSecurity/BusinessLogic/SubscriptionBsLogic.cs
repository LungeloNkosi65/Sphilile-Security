using SphileSecurity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SphileSecurity.BusinessLogic
{
    public class SubscriptionBsLogic
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static string GenerateReferenceNumber(PackageSubscription packageSubscription)
        {
            var packageName = db.SecurityPackages.Find(packageSubscription.SecurityPackageId);
            var reference = packageName.PackageName + packageSubscription.
                CustomerEmail.Substring(0, packageSubscription.CustomerEmail.LastIndexOf("@")) + packageSubscription.SecurityPackageId;
            return reference;
        }
    }
}