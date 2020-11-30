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
    public class AssignShiftsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AssignShifts
        public ActionResult Index()
        {
            var assignShifts = db.AssignShifts.Include(a => a.Employees).Include(a => a.Shift);
            return View(assignShifts.ToList());
        }

        // GET: AssignShifts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignShift assignShift = db.AssignShifts.Find(id);
            if (assignShift == null)
            {
                return HttpNotFound();
            }
            return View(assignShift);
        }

        // GET: AssignShifts/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Email");
            ViewBag.ShiftId = new SelectList(db.Shifts, "ShiftId", "ShiftName");
            return View();
        }

        // POST: AssignShifts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssignId,EmployeeId,ShiftId")] AssignShift assignShift)
        {
            if (ModelState.IsValid)
            {
                db.AssignShifts.Add(assignShift);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Email", assignShift.EmployeeId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "ShiftId", "ShiftName", assignShift.ShiftId);
            return View(assignShift);
        }

        // GET: AssignShifts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignShift assignShift = db.AssignShifts.Find(id);
            if (assignShift == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Email", assignShift.EmployeeId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "ShiftId", "ShiftName", assignShift.ShiftId);
            return View(assignShift);
        }

        // POST: AssignShifts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssignId,EmployeeId,ShiftId")] AssignShift assignShift)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignShift).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Email", assignShift.EmployeeId);
            ViewBag.ShiftId = new SelectList(db.Shifts, "ShiftId", "ShiftName", assignShift.ShiftId);
            return View(assignShift);
        }

        // GET: AssignShifts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignShift assignShift = db.AssignShifts.Find(id);
            if (assignShift == null)
            {
                return HttpNotFound();
            }
            return View(assignShift);
        }

        // POST: AssignShifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AssignShift assignShift = db.AssignShifts.Find(id);
            db.AssignShifts.Remove(assignShift);
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
