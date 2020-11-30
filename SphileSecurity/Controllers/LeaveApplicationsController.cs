using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SphileSecurity.IMfuyoRachLogic;
using SphileSecurity.Models;
using SphileSecurity.Services;

namespace SphileSecurity.Controllers
{
    public class LeaveApplicationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LeaveApplications
        public ActionResult Index()
        {
            return View(db.LeaveApplications.ToList());
        }

        // GET: LeaveApplications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveApplication leaveApplication = db.LeaveApplications.Find(id);
            if (leaveApplication == null)
            {
                return HttpNotFound();
            }
            return View(leaveApplication);
        }

        // GET: LeaveApplications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LeaveApplicationId,EmployeeEmail,FromDate,ToDate,DateApplied,LeaveApplicationStatus")] LeaveApplication leaveApplication)
        {
            if (ModelState.IsValid)
            {
                if(BILogic.CheckDate(leaveApplication.FromDate) && BILogic.CheckDate(leaveApplication.ToDate) && BILogic.CheckLeaveDates(leaveApplication.FromDate, leaveApplication.ToDate))
                {
                    leaveApplication.EmployeeEmail = User.Identity.GetUserName();
                    leaveApplication.LeaveApplicationStatus = "Awaiting Approval";
                    leaveApplication.DateApplied = DateTime.Now;
                    db.LeaveApplications.Add(leaveApplication);
                    db.SaveChanges();
                    EmailSender.LeaveApplication(leaveApplication);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid dates passed please select valid dates");
                    return View(leaveApplication);
                }
       
            }

            return View(leaveApplication);
        }

        // GET: LeaveApplications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveApplication leaveApplication = db.LeaveApplications.Find(id);
            if (leaveApplication == null)
            {
                return HttpNotFound();
            }
            return View(leaveApplication);
        }

        // POST: LeaveApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LeaveApplicationId,EmployeeEmail,FromDate,ToDate,DateApplied,LeaveApplicationStatus")] LeaveApplication leaveApplication)
        {
            if (ModelState.IsValid)
            {
                leaveApplication.EmployeeEmail = User.Identity.GetUserName();
                db.Entry(leaveApplication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(leaveApplication);
        }

        // GET: LeaveApplications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveApplication leaveApplication = db.LeaveApplications.Find(id);
            if (leaveApplication == null)
            {
                return HttpNotFound();
            }
            return View(leaveApplication);
        }

        // POST: LeaveApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LeaveApplication leaveApplication = db.LeaveApplications.Find(id);
            db.LeaveApplications.Remove(leaveApplication);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Approve(int? id)
        {
            BILogic.ChangeStatus(id, "Approved");
            return RedirectToAction("Index");
        }  
        public ActionResult Decline(int? id)
        {
            BILogic.ChangeStatus(id, "Decline");
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
