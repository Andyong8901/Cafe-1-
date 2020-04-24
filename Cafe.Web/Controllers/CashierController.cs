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

        public User CheckUser()
        {
            var CashierId = Convert.ToInt32(Session["CashierId"]);
            var CheckLogin = UserRepo.GetUser(CashierId);
            if (CheckLogin == null)
            {
                RedirectToAction("Login");
            }
            return CheckLogin;
        }


        // GET: Cashier
        public ActionResult Table()
        {

            CheckUser();
            return View(TableRepo.GetTables());
        }

        public ActionResult SelectedTable(int? id)
        {

            CheckUser();
        
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

            CheckUser();
            return View();
        }

        [HttpPost]
        public ActionResult CreateTable([Bind(Include = "TableId,TableNo,TableStatus")] Table table)
        {
            table.TableStatus = TableStatus.Empty;
            TableRepo.AddTable(table);
            return RedirectToAction("Table");
        }

        public ActionResult CheckTableNo(string TableNo)
        {
            
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


        public ActionResult DeleteTable(int id)
        {
            CheckUser();
            Table table = TableRepo.GetTable(id);
            return View(table);
        }
        [HttpPost, ActionName("DeleteTable")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var getTable = TableRepo.GetTable(id);
            var CheckOrder = CartRepo.GetTableCart(getTable.TableId);
            if (CheckOrder.Count() != 0)
            {
                CartRepo.RemoveCartList(id);
            }
            TableRepo.RemoveTable(getTable);
            return RedirectToAction("Table");
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
