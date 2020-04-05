using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cafe.Web.Models;

namespace Cafe.Web.Controllers
{
    public class CustomersController : Controller
    {
        private CreateDB db = new CreateDB();

        // GET: Customers
        public ActionResult Menu()
        {
            return View(db.Categories.ToList());
        }

        public ActionResult AddItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Item = db.Categories.Find(id);
            var CheckItem = db.OrderCarts.SingleOrDefault(c => c.CategoriesId == id);

            if (CheckItem != null)
            {
                CheckItem.Quantity++;
                CheckItem.TotalAmount = Item.UnitPrice * CheckItem.Quantity;
            }
            else
            {
                OrderCart ordercart = new OrderCart
                {
                    CategoriesId = Item.CategoriesId,
                    Quantity = 1
                };
                ordercart.TotalAmount = Item.UnitPrice * ordercart.Quantity;
                db.OrderCarts.Add(ordercart);
            }
            db.SaveChanges();
            return View(db.Categories.ToList());
        }

        public ActionResult ListCart()
        {
            return View(db.OrderCarts.ToList());
        }
        public ActionResult ClearAllItem()
        {
            db.OrderCarts.RemoveRange(db.OrderCarts.ToList());
            return View();
        }
        public ActionResult PlusItem(int? id)
        {
            var CheckItem = db.OrderCarts.SingleOrDefault(c => c.CategoriesId == id);
            var Item = db.Categories.Find(id);

            CheckItem.Quantity++;
            CheckItem.TotalAmount = Item.UnitPrice * CheckItem.Quantity;

            return View();
        }
        public ActionResult MinusItem(int? id)
        {
            var CheckItem = db.OrderCarts.SingleOrDefault(c => c.CategoriesId == id);
            var Item = db.Categories.Find(id);

            CheckItem.Quantity--;
            CheckItem.TotalAmount = Item.UnitPrice * CheckItem.Quantity;
            return View();
        }

        //// GET: Customers/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    User user = db.Users.Find(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

        //// GET: Customers/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Customers/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "UserId,Username,Password,Roles")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Users.Add(user);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(user);
        //}

        //// GET: Customers/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    User user = db.Users.Find(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

        //// POST: Customers/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "UserId,Username,Password,Roles")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(user).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(user);
        //}

        //// GET: Customers/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    User user = db.Users.Find(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

        //// POST: Customers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    User user = db.Users.Find(id);
        //    db.Users.Remove(user);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
