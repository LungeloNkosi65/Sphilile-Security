using SphileSecurity.Models;
using SphileSecurity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SphileSecurity.IMfuyoRachLogic
{
    public class BILogic
    {
        private static readonly ApplicationDbContext db = new ApplicationDbContext();
        private static decimal DiscountRate = 0;

        public static bool CheckDate(DateTime dateTime)
        {
            return dateTime > DateTime.Now;
        }

        public static bool CheckLeaveDates(DateTime fromDate, DateTime toDate)
        {
            return toDate > fromDate;
        }

        public static void ChangeStatus(int? id, string status)
        {
            var dbRecord = db.LeaveApplications.Find(id);
            dbRecord.LeaveApplicationStatus = status;
            db.Entry(dbRecord).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            EmailSender.LeaveApplication(dbRecord);
        }

        public static decimal GetDiscountRate(string userEmail)
        {
            try
            {
                var subscription = db.PackageSubscriptions.Where(x => x.CustomerEmail == userEmail).FirstOrDefault();

                if (subscription != null)
                {
                    var getPackage = db.SecurityPackages.Find(subscription.SecurityPackageId);
                    var discountRate = db.SubscriptionDiscounts.Where(x => x.PackageTypeId == getPackage.PackageTypeId).FirstOrDefault();
                    DiscountRate = discountRate.DiscountPercent / 100;
                    return discountRate.DiscountPercent;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        public static decimal CalcDiscountPrice(decimal itemPrice)
        {
            var discountPrice = itemPrice - (itemPrice * (DiscountRate));
            return discountPrice;
        }

        public static double CalcDiscountTotal(double total, double discountRate)
        {
            return (double)(total - (total * ((discountRate / 100))));
        }

        public static decimal GetHirePrice(int id)
        {
            var securityPackage = db.SecurityHireTypePackages.Find(id);
            return securityPackage.Price;
        }

        public static void HireStatus(int? id,string status)
        {
            var dbRecord = db.SecurityHires.Find(id);
            dbRecord.Status = status;
            db.Entry(dbRecord).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            EmailSender.SecurityHIre(dbRecord);

        }

    }
}