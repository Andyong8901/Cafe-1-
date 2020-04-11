using Cafe.DomainModelEntity;
using Cafe.InfrastructurePersistance.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static Cafe.DomainModelEntity.User;

namespace Cafe.Web.Controllers
{
    public class CashierController : Controller
    {
        UserRepository UserRepo = new UserRepository();
        CategoryRepository CategoryRepo = new CategoryRepository();
        TableRepository TableRepo = new TableRepository();
        OrderCartRepository CartRepo = new OrderCartRepository();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User Users)
        {
            //var admin = GetUser();
            Users.Roles = Role.Cashers;
            var Cashers = UserRepo.CheckUser(Users);
            if (Cashers != null)
            {
                if (Users.Password != Cashers.Password)
                {
                    ViewBag.error = "Invalid Username Or Password \nPlease Try Again";
                    return View();
                }
                else
                {
                    Session["CashierId"] = Cashers.UserId;
                    return RedirectToAction("Table");
                }
            }
            else
            {
                return View();
            }
        }
        // GET: Cashier
        public ActionResult Table()
        {
            var CashierId = Convert.ToInt32(Session["CashierId"]);
            var CheckLogin = UserRepo.GetUser(CashierId);
            if (CheckLogin == null)
            {
                return RedirectToAction("Login");
            }
            return View(TableRepo.GetTables());
        }

        public ActionResult SelectedTable(int? id)
        {
            var CashierId = Convert.ToInt32(Session["CashierId"]);
            var CheckLogin = UserRepo.GetUser(CashierId);
            if (CheckLogin == null)
            {
                return RedirectToAction("Login");
            }
            else if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tabledata = TableRepo.GetTable(id);
            return View(tabledata);
        }
        [HttpPost]
        public ActionResult SelectedTable([Bind(Include = "TableId,TableNo,TableStatus")] Table table)
        {
            var tabledata = TableRepo.GetTable(table.TableId);
            if (tabledata.TableStatus != table.TableStatus)
            {
                tabledata.TableStatus = table.TableStatus;
            }
            if (tabledata.TableStatus == TableStatus.Empty)
            {
            var CheckOrder = CartRepo.GetTableCart(tabledata.TableId);
                if (CheckOrder != null)
                {
                    CartRepo.RemoveCartList(tabledata.TableId);
                }
                tabledata.TotalQuantity = 0;
                tabledata.TotalPrice = 0;
                tabledata.UserId = null;
            }
            TableRepo.UpdateTable(tabledata);
            return RedirectToAction("Table");
        }

        public ActionResult CreateTable()
        {
            var CashierId = Convert.ToInt32(Session["CashierId"]);
            var CheckLogin = UserRepo.GetUser(CashierId);
            if (CheckLogin == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateTable([Bind(Include = "TableId,TableNo,TableStatus")] Table table)
        {
            table.TableStatus = TableStatus.Empty;
            TableRepo.AddTable(table);
            return View();
        }

        public ActionResult CheckTableNo(string TableNo)
        {
            //if (TableNo == "")
            //{
            //    var text = $"Please Enter Table Nomber";
            //    return Json(new { text, CheckNo = false }, JsonRequestBehavior.AllowGet);
            //}
            var Checking = TableRepo.CheckTableNo(TableNo);
            if (Checking != null)
            {
                var text = $"This Table Name Is Exist, Please Try Another Table Name";
                return Json(new { text, CheckNo = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { CheckNo = true }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return View();
        }

        // GET: Cashier/Details/5
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

        //// GET: Cashier/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Cashier/Create
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

        //// GET: Cashier/Edit/5
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

        //// POST: Cashier/Edit/5
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

        //// GET: Cashier/Delete/5
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

        //// POST: Cashier/Delete/5
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
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
