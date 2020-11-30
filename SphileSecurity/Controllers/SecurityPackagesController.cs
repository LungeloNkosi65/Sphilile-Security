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
    public class SecurityPackagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SecurityPackages
        public ActionResult Index()
        {
            var securityPackages = db.SecurityPackages.Include(s => s.PackageType);
            return View(securityPackages.ToList());
        }

        public ActionResult PackagesView(int? id)
        {
            var securityPackages = db.SecurityPackages.Include(s => s.PackageType);
            return View(securityPackages.ToList().Where(x=>x.PackageTypeId==id));
        }

        // GET: SecurityPackages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityPackage securityPackage = db.SecurityPackages.Find(id);
            if (securityPackage == null)
            {
                return HttpNotFound();
            }
            return View(securityPackage);
        }

        // GET: SecurityPackages/Create
        public ActionResult Create()
        {
            ViewBag.PackageTypeId = new SelectList(db.PackageTypes, "PackageTypeId", "PackageName");
            return View();
        }

        // POST: SecurityPackages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SecurityPackageId,PackageTypeId,PackageName,PackagePrice,TaxPercent,PackageDescription")] SecurityPackage securityPackage)
        {
            if (ModelState.IsValid)
            {
                db.SecurityPackages.Add(securityPackage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PackageTypeId = new SelectList(db.PackageTypes, "PackageTypeId", "PackageName", securityPackage.PackageTypeId);
            return View(securityPackage);
        }

        // GET: SecurityPackages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityPackage securityPackage = db.SecurityPackages.Find(id);
            if (securityPackage == null)
            {
                return HttpNotFound();
            }
            ViewBag.PackageTypeId = new SelectList(db.PackageTypes, "PackageTypeId", "PackageName", securityPackage.PackageTypeId);
            return View(securityPackage);
        }

        // POST: SecurityPackages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SecurityPackageId,PackageTypeId,PackageName,PackagePrice,TaxPercent,PackageDescription")] SecurityPackage securityPackage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(securityPackage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PackageTypeId = new SelectList(db.PackageTypes, "PackageTypeId", "PackageName", securityPackage.PackageTypeId);
            return View(securityPackage);
        }

        // GET: SecurityPackages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityPackage securityPackage = db.SecurityPackages.Find(id);
            if (securityPackage == null)
            {
                return HttpNotFound();
            }
            return View(securityPackage);
        }

        // POST: SecurityPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SecurityPackage securityPackage = db.SecurityPackages.Find(id);
            db.SecurityPackages.Remove(securityPackage);
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
