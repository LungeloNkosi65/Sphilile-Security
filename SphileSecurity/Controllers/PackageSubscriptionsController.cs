using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SphileSecurity.BusinessLogic;
using SphileSecurity.Models;
using SphileSecurity.Services;

namespace SphileSecurity.Controllers
{
    public class PackageSubscriptionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PackageSubscriptions
        public ActionResult Index()
        {
            var packageSubscriptions = db.PackageSubscriptions.Include(p => p.SecurityPackage);
            var userName = User.Identity.GetUserName();
            if (!User.IsInRole("Admin"))
            {
                return View(packageSubscriptions.ToList().Where(x=>x.CustomerEmail==userName));
            }
            return View(packageSubscriptions.ToList());
        }

        // GET: PackageSubscriptions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackageSubscription packageSubscription = db.PackageSubscriptions.Find(id);
            if (packageSubscription == null)
            {
                return HttpNotFound();
            }
            return View(packageSubscription);
        }

        // GET: PackageSubscriptions/Create
        public ActionResult Create(int? id)
        {
            Session["PackageSubscriptionId"] = id;
            ViewBag.SecurityPackageId = new SelectList(db.SecurityPackages, "SecurityPackageId", "PackageName");
            return View();
        }

        // POST: PackageSubscriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PackageSubscriptionId,SecurityPackageId,CustomerEmail,SubscriptionFee,SubStatus,DateSubscribed,SubReference")] PackageSubscription packageSubscription)
        {
            if (ModelState.IsValid)
            {
                var customerEmail = User.Identity.GetUserName();
                packageSubscription.SecurityPackageId = int.Parse(Session["PackageSubscriptionId"].ToString());
                packageSubscription.CustomerEmail = customerEmail;
                packageSubscription.SubscriptionFee = packageSubscription.getPackagePrice();
                packageSubscription.SubStatus = "Awaiting Approval";
                packageSubscription.DateSubscribed = DateTime.Now;
                packageSubscription.SubReference = SubscriptionBsLogic.GenerateReferenceNumber(packageSubscription);
                db.PackageSubscriptions.Add(packageSubscription);
                db.SaveChanges();
                EmailSender.SendSubscriptionConfrimations(packageSubscription);
                return RedirectToAction("Index");
            }

            ViewBag.SecurityPackageId = new SelectList(db.SecurityPackages, "SecurityPackageId", "PackageName", packageSubscription.SecurityPackageId);
            return View(packageSubscription);
        }
        public ActionResult CancellSubscription(int? id)
        {
            var dbRecord = db.PackageSubscriptions.Find(id);
            return View(dbRecord);
        }
        public ActionResult ConfirmSubscriptionCancel(int? id)
        {
            var dbRecord = db.PackageSubscriptions.Find(id);
            dbRecord.SubStatus = "Subscription Canceled";
            db.Entry(dbRecord).State = EntityState.Modified;
            db.SaveChanges();
            EmailSender.SubscriptionCancelled(dbRecord);
            return RedirectToAction("Index");
        }  
        public ActionResult Decline(int? id)
        {
            var dbRecord = db.PackageSubscriptions.Find(id);
            dbRecord.SubStatus = "Declined";
            db.Entry(dbRecord).State = EntityState.Modified;
            db.SaveChanges();
            EmailSender.SubscriptionCancelled(dbRecord);
            return RedirectToAction("Index");
        }
        public ActionResult ApproveSubscription(int? id)
        {
            var dbRecord = db.PackageSubscriptions.Find(id);
            dbRecord.SubStatus = "Approved";
            db.Entry(dbRecord).State = EntityState.Modified;
            db.SaveChanges();
            EmailSender.PayementDone(dbRecord);
            return RedirectToAction("Index");
        }
        public ActionResult PaySubscriptionFee(int? id)
        {
            var dbRecord = db.PackageSubscriptions.Find(id);
            dbRecord.SubStatus = "Paid And Active";
            db.Entry(dbRecord).State = EntityState.Modified;
            db.SaveChanges();
            EmailSender.PayementDone(dbRecord);
            return RedirectToAction("OnceOff","SubPayment", new { amount=dbRecord.SubscriptionFee});
        }
        // GET: PackageSubscriptions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackageSubscription packageSubscription = db.PackageSubscriptions.Find(id);
            if (packageSubscription == null)
            {
                return HttpNotFound();
            }
            ViewBag.SecurityPackageId = new SelectList(db.SecurityPackages, "SecurityPackageId", "PackageName", packageSubscription.SecurityPackageId);
            return View(packageSubscription);
        }

        // POST: PackageSubscriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PackageSubscriptionId,SecurityPackageId,CustomerEmail,SubscriptionFee,SubStatus,DateSubscribed")] PackageSubscription packageSubscription)
        {
            if (ModelState.IsValid)
            {
                db.Entry(packageSubscription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SecurityPackageId = new SelectList(db.SecurityPackages, "SecurityPackageId", "PackageName", packageSubscription.SecurityPackageId);
            return View(packageSubscription);
        }

        // GET: PackageSubscriptions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackageSubscription packageSubscription = db.PackageSubscriptions.Find(id);
            if (packageSubscription == null)
            {
                return HttpNotFound();
            }
            return View(packageSubscription);
        }

        // POST: PackageSubscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PackageSubscription packageSubscription = db.PackageSubscriptions.Find(id);
            db.PackageSubscriptions.Remove(packageSubscription);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
