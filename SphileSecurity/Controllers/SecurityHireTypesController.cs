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
    public class SecurityHireTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SecurityHireTypes
        public ActionResult Index()
        {
            return View(db.SecurityHireTypes.ToList());
        }

        // GET: SecurityHireTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityHireType securityHireType = db.SecurityHireTypes.Find(id);
            if (securityHireType == null)
            {
                return HttpNotFound();
            }
            return View(securityHireType);
        }

        // GET: SecurityHireTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SecurityHireTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SecurityHireTypeId,SecurityHireTypeName")] SecurityHireType securityHireType)
        {
            if (ModelState.IsValid)
            {
                db.SecurityHireTypes.Add(securityHireType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(securityHireType);
        }

        // GET: SecurityHireTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityHireType securityHireType = db.SecurityHireTypes.Find(id);
            if (securityHireType == null)
            {
                return HttpNotFound();
            }
            return View(securityHireType);
        }

        // POST: SecurityHireTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SecurityHireTypeId,SecurityHireTypeName")] SecurityHireType securityHireType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(securityHireType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(securityHireType);
        }

        // GET: SecurityHireTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityHireType securityHireType = db.SecurityHireTypes.Find(id);
            if (securityHireType == null)
            {
                return HttpNotFound();
            }
            return View(securityHireType);
        }

        // POST: SecurityHireTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SecurityHireType securityHireType = db.SecurityHireTypes.Find(id);
            db.SecurityHireTypes.Remove(securityHireType);
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
