using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cafe.Web.Models;
using Cafe.Web.ViewModel;
using static Cafe.Web.Models.User;

namespace Cafe.Web.Controllers
{
    public class CustomersController : Controller
    {
        private CreateDB db = new CreateDB();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginVM loginVM)
        {
            var customer = db.Users.SingleOrDefault(a => a.Username == loginVM.Username && a.Roles == Role.Customer);

            if (loginVM.Password != customer.Password)
            {
                ViewBag.Error = "Invalid Username Or Password \nPlease Try Again";
                return View();
            }
            else
            {
                Session["customerId"] = customer.UserId;
                return RedirectToAction("SelectTable");
            }
        }
        // GET: Customers
        public ActionResult SelectTable()
        {
            var CustomerId = Convert.ToInt32(Session["customerId"]);
            var CheckLogin = db.Users.SingleOrDefault(u => u.UserId == CustomerId);
            if (CheckLogin == null)
            {
                return RedirectToAction("Menu");
            }
            var CheckTable = db.Tables.Where(t => t.UserId == CustomerId).ToList();
            if (CheckTable.Count() != 0)
            {
                return RedirectToAction("Menu");
            }
            else
            {
                var ListTable = db.Tables.Where(t => t.TableStatus != TableStatus.Occupied).ToList();
                return View(ListTable);
            }
        }

        public ActionResult SelectedTable(int? id)
        {
            var CustomerId = Convert.ToInt32(Session["customerId"]);
            var CheckTable = db.Tables.Where(t => t.UserId == CustomerId).ToList();
            if (CheckTable.Count() != 0)
            {
                return RedirectToAction("Menu");
            }
            var ListTable = db.Tables.SingleOrDefault(t => t.TableId == id);
            if (ListTable != null)
            {
                ListTable.TableStatus = TableStatus.Occupied;
                ListTable.UserId = CustomerId;
                db.SaveChanges();
                return RedirectToAction("Menu");
            }
            return View();
        }

        public ActionResult Menu()
        {
            var CustomerId = Convert.ToInt32(Session["customerId"]);
            var CheckLogin = db.Users.SingleOrDefault(u=>u.UserId == CustomerId);
            if (CheckLogin == null)
            {
                return RedirectToAction("Menu");
            }

            var UserTable = db.Tables.SingleOrDefault(t => t.UserId == CustomerId);

            LoopItem(UserTable);
            ViewBag.Name = UserTable.User.Username;
            ViewBag.TableNo = UserTable.TableNo;
            return View(db.Categories.ToList());
        }
        
        public ActionResult AddItem(int? id)
        {
            var CustomerId = Convert.ToInt32(Session["customerId"]);
            var CheckLogin = db.Users.SingleOrDefault(u => u.UserId == CustomerId);
            if (CheckLogin == null)
            {
                return RedirectToAction("Menu");
            }
            var Item = db.Categories.Find(id);
            var CheckTable = db.Tables.SingleOrDefault(t => t.UserId == CustomerId);
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
                    Quantity = 1,
                };
                ordercart.TotalAmount = Item.UnitPrice * ordercart.Quantity;
                ordercart.TableId = CheckTable.TableId;
                db.OrderCarts.Add(ordercart);
            }
            db.SaveChanges();
            return RedirectToAction("Menu");
        }

        public ActionResult ListCart()
        {
            var CustomerId = Convert.ToInt32(Session["customerId"]);
            var CheckLogin = db.Users.SingleOrDefault(u => u.UserId == CustomerId);
            if (CheckLogin == null)
            {
                return RedirectToAction("Menu");
            }
            var CheckTable = db.Tables.SingleOrDefault(t => t.UserId == CustomerId);
            LoopItem(CheckTable);
            var CustomerCart = db.OrderCarts.Where(o => o.Table.UserId == CustomerId && o.TableId == CheckTable.TableId).ToList();
            return View(CustomerCart);
        }
        public ActionResult ClearAllItem()
        {
            var CustomerId = Convert.ToInt32(Session["customerId"]);
            var CheckLogin = db.Users.SingleOrDefault(u => u.UserId == CustomerId);
            if (CheckLogin == null)
            {
                return RedirectToAction("Menu");
            }
            var CheckTable = db.Tables.SingleOrDefault(t => t.UserId == CustomerId);
            var customerClear = db.OrderCarts.Where(o => o.Table.UserId == CustomerId && o.TableId == CheckTable.TableId).ToList();
            db.OrderCarts.RemoveRange(customerClear);
            db.SaveChanges();
            return RedirectToAction("Menu");
        }
        public ActionResult PlusItem(int? id)
        {
            var CustomerId = Convert.ToInt32(Session["customerId"]);
            var CheckItem = db.OrderCarts.SingleOrDefault(o => o.OrdercartId == id && o.Table.UserId == CustomerId);

            CheckItem.Quantity++;
            CheckItem.TotalAmount = CheckItem.Categories.UnitPrice * CheckItem.Quantity;
            db.SaveChanges();

            return RedirectToAction("ListCart");
        }
        public ActionResult MinusItem(int? id)
        {
            var CustomerId = Convert.ToInt32(Session["customerId"]);
            var CheckItem = db.OrderCarts.SingleOrDefault(o => o.OrdercartId == id && o.Table.UserId == CustomerId);
            CheckItem.Quantity--;
            if (CheckItem.Quantity == 0)
            {
                db.OrderCarts.Remove(CheckItem);
            }
            else
            {
                CheckItem.TotalAmount = CheckItem.Categories.UnitPrice * CheckItem.Quantity;
            }
            db.SaveChanges();

            return RedirectToAction("ListCart");
        }

        [NonAction]
        public ActionResult LoopItem(Table UserTable)
        {
            var totalQuantity = db.OrderCarts.Where(o => o.Table.TableId == UserTable.TableId).ToList();
            var TotalQuan = 0;
            foreach (var item in totalQuantity)
            {
                TotalQuan += item.Quantity;
            }
            return ViewBag.TotalQuantity = TotalQuan;

        }

        public ActionResult Logout()
        {
            Session.Abandon();
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
