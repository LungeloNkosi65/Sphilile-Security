using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SphileSecurity.BusinessLogic;
using SphileSecurity.Models;

namespace SphileSecurity.Controllers
{
    public class ShiftsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Shifts
        public ActionResult Index()
        {
            var shifts = db.Shifts.Include(s => s.ShiftType);
            return View(shifts.ToList());
        }

        // GET: Shifts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = db.Shifts.Find(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // GET: Shifts/Create
        public ActionResult Create()
        {
            ViewBag.ShiftTypeId = new SelectList(db.ShiftTypes, "ShiftTypeId", "ShiftTypeName");
            return View();
        }

        // POST: Shifts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShiftId,ShiftTypeId,ShiftHours,TimeIn,TimeOut,PayRatePerHour,ShiftName")] Shift shift)
        {
            if (ModelState.IsValid)
            {
                shift.ShiftHours = ShiftbusinessLogic.CalcNoOfHours(shift.TimeIn, shift.TimeOut);
                db.Shifts.Add(shift);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ShiftTypeId = new SelectList(db.ShiftTypes, "ShiftTypeId", "ShiftTypeName", shift.ShiftTypeId);
            return View(shift);
        }

        // GET: Shifts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = db.Shifts.Find(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShiftTypeId = new SelectList(db.ShiftTypes, "ShiftTypeId", "ShiftTypeName", shift.ShiftTypeId);
            return View(shift);
        }

        // POST: Shifts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShiftId,ShiftTypeId,ShiftHours,TimeIn,TimeOut,PayRatePerHour,ShiftName")] Shift shift)
        {
            if (ModelState.IsValid)
            {
                shift.ShiftHours = ShiftbusinessLogic.CalcNoOfHours(shift.TimeIn, shift.TimeOut);
                db.Entry(shift).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ShiftTypeId = new SelectList(db.ShiftTypes, "ShiftTypeId", "ShiftTypeName", shift.ShiftTypeId);
            return View(shift);
        }

        // GET: Shifts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = db.Shifts.Find(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // POST: Shifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shift shift = db.Shifts.Find(id);
            db.Shifts.Remove(shift);
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
