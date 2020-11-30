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
    public class SecurityHiresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static string userName;

        public void SetEmail()
        {
            userName = User.Identity.GetUserName();
        }
        // GET: SecurityHires
        public ActionResult Index()
        {
            SetEmail();
            var securityHires = db.SecurityHires.Include(s => s.SecurityHireTypePackages);
            if (User.IsInRole("Admin"))
            {
                return View(securityHires.ToList());
            }
            else
            {
                return View(securityHires.ToList().Where(x=>x.UserEmail==userName));
            }
            
        }

        // GET: SecurityHires/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityHire securityHire = db.SecurityHires.Find(id);
            if (securityHire == null)
            {
                return HttpNotFound();
            }
            return View(securityHire);
        }

        // GET: SecurityHires/Create
        public ActionResult Create(int id)
        {
            Session["PAckageId"] = id;
            ViewBag.SecurityHireTypePackagesId = new SelectList(db.SecurityHireTypePackages, "SecurityHireTypePackagesId", "Name");
            return View();
        }

        // POST: SecurityHires/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SecurityHireId,SecurityHireTypePackagesId,UserEmail,DateHired,DateFor,HirePrice,Status")] SecurityHire securityHire)
        {
            SetEmail();
            if (ModelState.IsValid)
            {
                if (BILogic.CheckDate(securityHire.DateFor))
                {
                    securityHire.UserEmail = userName;
                    securityHire.DateHired = DateTime.Now;
                    securityHire.Status = "Awaiting Approval";
                    securityHire.SecurityHireTypePackagesId = int.Parse(Session["PAckageId"].ToString());
                    securityHire.HirePrice = BILogic.GetHirePrice(securityHire.SecurityHireTypePackagesId);
                    db.SecurityHires.Add(securityHire);
                    db.SaveChanges();
                    EmailSender.SecurityHIre(securityHire);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "You can not select a date  that has already passed");
                    ViewBag.SecurityHireTypePackagesId = new SelectList(db.SecurityHireTypePackages, "SecurityHireTypePackagesId", "Name", securityHire.SecurityHireTypePackagesId);
                    return View(securityHire);
                }
               
            }

            ViewBag.SecurityHireTypePackagesId = new SelectList(db.SecurityHireTypePackages, "SecurityHireTypePackagesId", "Name", securityHire.SecurityHireTypePackagesId);
            return View(securityHire);
        }

        // GET: SecurityHires/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityHire securityHire = db.SecurityHires.Find(id);
            if (securityHire == null)
            {
                return HttpNotFound();
            }
            ViewBag.SecurityHireTypePackagesId = new SelectList(db.SecurityHireTypePackages, "SecurityHireTypePackagesId", "Name", securityHire.SecurityHireTypePackagesId);
            return View(securityHire);
        }

        // POST: SecurityHires/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SecurityHireId,SecurityHireTypePackagesId,UserEmail,DateHired,DateFor,HirePrice,Status")] SecurityHire securityHire)
        {
            if (ModelState.IsValid)
            {
                securityHire.UserEmail = userName;
                securityHire.DateHired = DateTime.Now;
                db.Entry(securityHire).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SecurityHireTypePackagesId = new SelectList(db.SecurityHireTypePackages, "SecurityHireTypePackagesId", "Name", securityHire.SecurityHireTypePackagesId);
            return View(securityHire);
        }

        // GET: SecurityHires/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityHire securityHire = db.SecurityHires.Find(id);
            if (securityHire == null)
            {
                return HttpNotFound();
            }
            return View(securityHire);
        }

        // POST: SecurityHires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SecurityHire securityHire = db.SecurityHires.Find(id);
            db.SecurityHires.Remove(securityHire);
            db.SaveChanges();
            return RedirectToAction("Index");
        }    
        public ActionResult Approve(int? id)
        {
            BILogic.HireStatus(id, "Approved");
            return RedirectToAction("Index");
        }  

        public ActionResult Decline(int? id)
        {
            BILogic.HireStatus(id, "Declined");
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
