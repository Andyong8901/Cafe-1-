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
    public class CashierController : Controller
    {
        private CreateDB db = new CreateDB();
        public void PreloadTable()
        {
            List<Table> tables = new List<Table>()
            {
                new Table(){ TableNo="T1",TableStatus=TableStatus.Empty },
                new Table(){ TableNo="T2",TableStatus=TableStatus.Empty },
                new Table(){ TableNo="T3",TableStatus=TableStatus.Empty },
                new Table(){ TableNo="T4",TableStatus=TableStatus.Empty }
            };
            if (db.Tables.Count() == 0)
            {
                db.Tables.AddRange(tables);
                db.SaveChanges();
            }
            else
            {
                return;
            }
        }
        // GET: Cashier
        public ActionResult Table()
        {
            return View(db.Tables.ToList());
        }

        public ActionResult SelectedTable(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tabledata = db.Tables.Find(id);
            return View(tabledata);
        }
        [HttpPost]
        public ActionResult SelectedTable([Bind(Include = "TableId,TableNo,TableStatus")] Table table)
        {
            var tabledata = db.Tables.SingleOrDefault(t => t.TableId == table.TableId);
            if (table.TableStatus != tabledata.TableStatus)
            {
                tabledata.TableStatus = table.TableStatus;
            }
            return View(tabledata);
        }

        public ActionResult CreateTable()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTable([Bind(Include = "TableId,TableNo,TableStatus")] Table table)
        {
            table.TableStatus = TableStatus.Empty;
            db.Tables.Add(table);
            db.SaveChanges();
            return View();
        }

        public ActionResult CheckTableNo(string TableNo)
        {
            if (TableNo == "")
            {
                var text = $"Please Enter Table Nomber";
                return Json(new { text, CheckType = false }, JsonRequestBehavior.AllowGet);
            }
            var Checking = db.Tables.SingleOrDefault(T => T.TableNo == TableNo);
            if (Checking != null)
            {
                var text = $"Incorrect Account Number";
                return Json(new { text, CheckNo = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { CheckNo = true }, JsonRequestBehavior.AllowGet);
            }
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
