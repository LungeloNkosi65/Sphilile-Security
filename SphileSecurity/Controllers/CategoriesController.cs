using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ImfuyoRanch.Controllers.Extensions;
using ImfuyoRanch.Models;
using ImfuyoRanch.Models.Business;

namespace ImfuyoRanch.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private DepartmentBusiness dbuss = new DepartmentBusiness();


        public ActionResult Index(string sortOrder, string searchString)
        {
            var students = from s in dbuss.GetDepartments()
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Department_Name.Contains(searchString)
                                       || s.Description.Contains(searchString));
                return View(students.ToList());

            }
            return View(dbuss.GetDepartments());
        }
        public ActionResult Details(int id)
        {

            if (dbuss.GetDepartment(id) != null)
                return View(dbuss.GetDepartment(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department model)
        {
            if (ModelState.IsValid)
            {
                dbuss.AddDepartment(model);
                this.AddToastMessage("Success", "Department succesfully created", Models.HelperToast.ToastType.Success);
                return RedirectToAction("Index");
            }

            return View(model);
        }
        public ActionResult Edit(int id)
        {

            if (dbuss.GetDepartment(id) != null)
                return View(dbuss.GetDepartment(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department model)
        {
            if (ModelState.IsValid)
            {
                dbuss.UpdateDepartment(model);
                this.AddToastMessage("Success", "Department updated succesfully", Models.HelperToast.ToastType.Success);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            try
            {
                var update = dbuss.RemoveDepartment(dbuss.GetDepartment(id));
                this.AddToastMessage("Success", "Department succesfully deleted", Models.HelperToast.ToastType.Success);
            }
            catch
            {
            }
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
    }
}
