﻿using Cafe.DomainModelEntity;
using Cafe.InfrastructurePersistance.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Cafe.Web.Controllers
{
    public class CategoriesController : Controller
    {

        UserRepository UserRepo = new UserRepository();
        CategoryRepository CategoryRepo = new CategoryRepository();
        TableRepository TableRepo = new TableRepository();
        OrderCartRepository CartRepo = new OrderCartRepository();
        // GET: Categories
        public ActionResult Index()
        {
            var Id = Convert.ToInt32(Session["AdminId"]);

            var checkAdmin = UserRepo.GetUser(Id);
            if (checkAdmin == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Name = checkAdmin.Username;
            //if (CheckCategory != null)
            //{
            //    ViewBag.CheckItem = "Found";
            //}
            //if (CheckCategory == null)
            //{
            //    ViewBag.CheckItem = null;
            //}
            return View(CategoryRepo.GetCategories());
        }


        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            var Id = Convert.ToInt32(Session["AdminId"]);
            var checkAdmin = UserRepo.GetUser(Id);
            if (checkAdmin == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Name = checkAdmin.Username;
            Categories categories = CategoryRepo.GetCategory(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            var Id = Convert.ToInt32(Session["AdminId"]);
            var checkAdmin = UserRepo.GetUser(Id);
        
            ViewBag.Name = checkAdmin.Username;
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoriesId,CategoryName,FoodImg,FoodName,UnitPrice,Remark")] Categories categories, HttpPostedFileBase Img)
        {
            if (ModelState.IsValid)
            {
                if (Img != null)
                {
                    categories.FoodImg = new byte[Img.ContentLength];
                    Img.InputStream.Read(categories.FoodImg, 0, Img.ContentLength);
                }
                if (categories.FoodImg == null)
                {
                    ViewBag.ErrorImg = "Please Upload Your Photo";
                    return View();
                }
                CategoryRepo.AddCategory(categories);
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            var Id = Convert.ToInt32(Session["AdminId"]);
            var checkAdmin = UserRepo.GetUser(Id);
            if (checkAdmin == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Name = checkAdmin.Username;
            Categories categories = CategoryRepo.GetCategory(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoriesId,CategoryName,FoodImg,FoodName,UnitPrice,Remark")] Categories categories)
        {
            var FindCategory = CategoryRepo.GetCategory(categories.CategoriesId);
            if (categories.FoodImg == null)
            {
                categories.FoodImg = FindCategory.FoodImg;
            }
            FindCategory.CategoryName = categories.CategoryName;
            FindCategory.FoodName = categories.FoodName;
            FindCategory.UnitPrice = categories.UnitPrice;
            FindCategory.Remark = categories.Remark;
            CategoryRepo.UpdateCategory(FindCategory);
            return RedirectToAction("Index");
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            var Id = Convert.ToInt32(Session["AdminId"]);
            var checkAdmin = UserRepo.GetUser(Id);
            if (checkAdmin == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Name = checkAdmin.Username;
            Categories categories = CategoryRepo.GetCategory(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = CategoryRepo.GetCategory(id);
            CategoryRepo.RemoveCategory(categories);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
