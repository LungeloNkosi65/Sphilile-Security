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
    public class SecurityHireTypePackagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SecurityHireTypePackages
        public ActionResult Index()
        {
            var securityHireTypePackages = db.SecurityHireTypePackages.Include(s => s.SecurityHireType);
            return View(securityHireTypePackages.ToList());
        }
          public ActionResult IndexView(int id)
        {
            var securityHireTypePackages = db.SecurityHireTypePackages.Include(s => s.SecurityHireType);
            return View(securityHireTypePackages.ToList().Where(x=>x.SecurityHireTypeId==id));
        }

        // GET: SecurityHireTypePackages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityHireTypePackages securityHireTypePackages = db.SecurityHireTypePackages.Find(id);
            if (securityHireTypePackages == null)
            {
                return HttpNotFound();
            }
            return View(securityHireTypePackages);
        }

        // GET: SecurityHireTypePackages/Create
        public ActionResult Create()
        {
            ViewBag.SecurityHireTypeId = new SelectList(db.SecurityHireTypes, "SecurityHireTypeId", "SecurityHireTypeName");
            return View();
        }

        // POST: SecurityHireTypePackages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SecurityHireTypePackagesId,SecurityHireTypeId,Description,Price,Rating,Name")] SecurityHireTypePackages securityHireTypePackages)
        {
            if (ModelState.IsValid)
            {
                securityHireTypePackages.Rating = 0;
                db.SecurityHireTypePackages.Add(securityHireTypePackages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SecurityHireTypeId = new SelectList(db.SecurityHireTypes, "SecurityHireTypeId", "SecurityHireTypeName", securityHireTypePackages.SecurityHireTypeId);
            return View(securityHireTypePackages);
        }

        // GET: SecurityHireTypePackages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityHireTypePackages securityHireTypePackages = db.SecurityHireTypePackages.Find(id);
            if (securityHireTypePackages == null)
            {
                return HttpNotFound();
            }
            ViewBag.SecurityHireTypeId = new SelectList(db.SecurityHireTypes, "SecurityHireTypeId", "SecurityHireTypeName", securityHireTypePackages.SecurityHireTypeId);
            return View(securityHireTypePackages);
        }

        // POST: SecurityHireTypePackages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SecurityHireTypePackagesId,SecurityHireTypeId,Description,Price,Rating,Name")] SecurityHireTypePackages securityHireTypePackages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(securityHireTypePackages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SecurityHireTypeId = new SelectList(db.SecurityHireTypes, "SecurityHireTypeId", "SecurityHireTypeName", securityHireTypePackages.SecurityHireTypeId);
            return View(securityHireTypePackages);
        }

        // GET: SecurityHireTypePackages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityHireTypePackages securityHireTypePackages = db.SecurityHireTypePackages.Find(id);
            if (securityHireTypePackages == null)
            {
                return HttpNotFound();
            }
            return View(securityHireTypePackages);
        }

        // POST: SecurityHireTypePackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SecurityHireTypePackages securityHireTypePackages = db.SecurityHireTypePackages.Find(id);
            db.SecurityHireTypePackages.Remove(securityHireTypePackages);
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
