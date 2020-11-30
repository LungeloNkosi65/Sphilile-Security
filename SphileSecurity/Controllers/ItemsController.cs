﻿using ImfuyoRanch.Models.Business;
using ImfuyoRanch.Models;
using ImfuyoRanch.Controllers.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ImfuyoRanch.Controllers
{
    public class ItemsController : Controller
    {
        private ItemBusiness item = new ItemBusiness();

        DepartmentBusiness category_Service = new DepartmentBusiness();
        ApplicationDbContext appDb = new ApplicationDbContext();
       
        public ActionResult Index()
        {
            return View(item.GetItems());
        }
        public ActionResult Details(int id)
        {
            
            if (item.GetItem(id) != null)
                return View(item.GetItem(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult Create()
        {
            ViewBag.Department_ID = new SelectList(category_Service.GetDepartments(), "Department_ID", "Department_Name");
            ViewBag.SupplierId = new SelectList(appDb.Suppliers.ToList(), "SupplierId", "SupplierName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "img_upload")] Item model)
        {
            ViewBag.Department_ID = new SelectList(category_Service.GetDepartments(), "Department_ID", "Department_Name");
            ViewBag.SupplierId = new SelectList(appDb.Suppliers.ToList(), "SupplierId", "SupplierName");
            if (ModelState.IsValid)
            {
                byte[] Pic = null;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase poImgFile = Request.Files["img_upload"];

                    using (var binary = new BinaryReader(poImgFile.InputStream))
                    {
                        Pic = binary.ReadBytes(poImgFile.ContentLength);
                    }
                }
                model.Picture = Pic;
                item.AddItem(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            if (item.GetItem(id) != null)
            {
                ViewBag.Department_ID = new SelectList(category_Service.GetDepartments(), "Department_ID", "Department_Name", item.GetItem(id).Department_ID);
                return View(item.GetItem(id));
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item model, HttpPostedFileBase img_upload)
        {
            if (img_upload != null)
            {
                string ext = Path.GetExtension(img_upload.FileName);
                if (ext != ".png" && ext != ".PNG" && ext != ".jpg" && ext != ".JPG" && ext != ".jpeg" && ext != ".JPEG")
                {
                }
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Content/Images/"), Guid.NewGuid().ToString() + Path.GetExtension(img_upload.FileName));
                    img_upload.SaveAs(path);
                    model.ImgPath = path.Substring(path.LastIndexOf("\\") + 1);
                }
                catch (Exception e)
                {
                    var err = e.Message;
                }
            }
            if (ModelState.IsValid)
            {
                item.UpdateItem(model);
                return RedirectToAction("Index");
            }
            ViewBag.Department_ID = new SelectList(category_Service.GetDepartments(), "Department_ID", "Department_Name",model.Department_ID);
            return View(model);
        }
        public JsonResult Delete(int id)
        {
            try
            {
                var update = item.RemoveItem(item.GetItem(id));
                this.AddToastMessage("Success", "Department succesfully deleted", ImfuyoRanch.Models.HelperToast.ToastType.Success);
            }
            catch
            {
            }
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }

        //Statistics below

        public ActionResult ItemStats()
        {
            return View();
        }
        public ActionResult ProfitChart()
        {
            ApplicationDbContext Mealcontext2 = new ApplicationDbContext();
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();

            var profits = (from p in Mealcontext2.Items select p);
            profits.ToList().ForEach(pr => xValue.Add(pr.Name));
            //profits.ToList().ForEach(pr => yValue.Add(pr.calculateCostDifference()));

            new Chart(width: 700, height: 450, theme: ChartTheme.Blue)
                .AddTitle("Profit Made Per Meal")
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue)
                .SetXAxis(title: "Meal Name")
                .SetYAxis(title: "Profit in Rands")
                .Write("jpeg");

            return null;

        }
        public ActionResult SalePriceChart()
        {
            ApplicationDbContext Mealcontext2 = new ApplicationDbContext();
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();

            var profits = (from p in Mealcontext2.Items select p);
            profits.ToList().ForEach(pr => xValue.Add(pr.Name));
            profits.ToList().ForEach(pr => yValue.Add(pr.Price));

            new Chart(width: 700, height: 450, theme: ChartTheme.Blue)
                .AddTitle("Sale Price Per Meal")
                .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue)
                .SetXAxis(title: "Meal Name")
                .SetYAxis(title: "Sale Price in Rands")
                .Write("jpeg");

            return null;

        }
    }
}