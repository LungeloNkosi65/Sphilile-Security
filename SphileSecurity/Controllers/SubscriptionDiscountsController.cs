using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SphileSecurity.Models;

namespace SphileSecurity.Controllers
{
    public class SubscriptionDiscountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SubscriptionDiscounts
        public ActionResult Index()
        {
            var subscriptionDiscounts = db.SubscriptionDiscounts.Include(s => s.PackageType);
            return View(subscriptionDiscounts.ToList());
        }

        // GET: SubscriptionDiscounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionDiscount subscriptionDiscount = db.SubscriptionDiscounts.Find(id);
            if (subscriptionDiscount == null)
            {
                return HttpNotFound();
            }
            return View(subscriptionDiscount);
        }

        // GET: SubscriptionDiscounts/Create
        public ActionResult Create()
        {
            ViewBag.PackageTypeId = new SelectList(db.PackageTypes, "PackageTypeId", "PackageName");
            return View();
        }

        // POST: SubscriptionDiscounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubscriptionDiscountId,PackageTypeId,DiscountPercent,NumberOfOrders")] SubscriptionDiscount subscriptionDiscount)
        {
            if (ModelState.IsValid)
            {
                db.SubscriptionDiscounts.Add(subscriptionDiscount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PackageTypeId = new SelectList(db.PackageTypes, "PackageTypeId", "PackageName", subscriptionDiscount.PackageTypeId);
            return View(subscriptionDiscount);
        }

        // GET: SubscriptionDiscounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionDiscount subscriptionDiscount = db.SubscriptionDiscounts.Find(id);
            if (subscriptionDiscount == null)
            {
                return HttpNotFound();
            }
            ViewBag.PackageTypeId = new SelectList(db.PackageTypes, "PackageTypeId", "PackageName", subscriptionDiscount.PackageTypeId);
            return View(subscriptionDiscount);
        }

        // POST: SubscriptionDiscounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubscriptionDiscountId,PackageTypeId,DiscountPercent,NumberOfOrders")] SubscriptionDiscount subscriptionDiscount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subscriptionDiscount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PackageTypeId = new SelectList(db.PackageTypes, "PackageTypeId", "PackageName", subscriptionDiscount.PackageTypeId);
            return View(subscriptionDiscount);
        }

        // GET: SubscriptionDiscounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionDiscount subscriptionDiscount = db.SubscriptionDiscounts.Find(id);
            if (subscriptionDiscount == null)
            {
                return HttpNotFound();
            }
            return View(subscriptionDiscount);
        }

        // POST: SubscriptionDiscounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubscriptionDiscount subscriptionDiscount = db.SubscriptionDiscounts.Find(id);
            db.SubscriptionDiscounts.Remove(subscriptionDiscount);
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
