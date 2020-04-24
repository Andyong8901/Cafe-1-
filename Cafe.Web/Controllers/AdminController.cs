using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cafe.DomainModelEntity;
using static Cafe.DomainModelEntity.User;
using Cafe.InfrastructurePersistance.Repository;

namespace Cafe.Web.Controllers
{
    public class AdminController : Controller
    {
        UserRepository UserRepo = new UserRepository();
        CategoryRepository CategoryRepo = new CategoryRepository();
        TableRepository TableRepo = new TableRepository();
        OrderCartRepository CartRepo = new OrderCartRepository();
        public void PreloadData()
        {
            List<User> users = new List<User>()
            {
                new User(){Username = "John",Password="123456",Roles=Role.Admin },
                new User(){Username = "John",Password="123456",Roles=Role.Cashers },
                new User(){Username = "John",Password="123456",Roles=Role.Customer }
            };
            List<Table> tables = new List<Table>()
            {
                new Table(){TableNo="T1",TableStatus=TableStatus.Empty,TotalQuantity=0,TotalPrice=0},
                new Table(){TableNo="T2",TableStatus=TableStatus.Empty,TotalQuantity=0,TotalPrice=0},
                new Table(){TableNo="T3",TableStatus=TableStatus.Empty,TotalQuantity=0,TotalPrice=0},
                new Table(){TableNo="T4",TableStatus=TableStatus.Empty,TotalQuantity=0,TotalPrice=0},
            };
            if (UserRepo.GetUsers().Count() == 0)
            {
                UserRepo.AddUserList(users);
            }

            if (TableRepo.GetTables().Count() == 0)
            {
                TableRepo.AddTableList(tables);
            }
        }
        public ActionResult Login()
        {
            PreloadData();
            return View();
        }
        [HttpPost]
        public ActionResult Login(User Users)
        {
            //var admin = GetUser();
            Users.Roles = Role.Admin;
            var Admin = UserRepo.CheckUser(Users);
            if (Admin != null)
            {
                if (Users.Password != Admin.Password)
                {
                    ViewBag.error = "Invalid Username Or Password \nPlease Try Again";
                    return View();
                }
                else
                {
                    Session["AdminId"] = Admin.UserId;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View();
            }
        }

        public User CheckUser()
        {
            var AdminId = Convert.ToInt32(Session["AdminId"]);
            var CheckLogin = UserRepo.GetUser(AdminId);
           
            return CheckLogin;
        }

        // GET: Admin
        public ActionResult Index()
        {
            var checkAdmin = CheckUser();
            ViewBag.Name = checkAdmin.Username;
            return View(UserRepo.GetUsers());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            var checkAdmin = CheckUser();
            ViewBag.Name = checkAdmin.Username;

            User user = UserRepo.GetUser(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            var checkAdmin = CheckUser();
            ViewBag.Name = checkAdmin.Username;
            return View();
        }


        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Username,Password,Roles")] User user)
        {
            if (ModelState.IsValid)
            {
                UserRepo.AddUser(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {

            var checkAdmin = CheckUser();
            ViewBag.Name = checkAdmin.Username;
            User user = UserRepo.GetUser(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult CheckUser(string Username, Role role, int? Id)
        {
            User CheckUser;
            CheckUser = UserRepo.FilterUser(Username, role, Id);

            if (CheckUser != null)
            {
                var text = "This Username Is Exist For This Roles, Please Try Another Username Or Roles";
                return Json(new { text, CheckUser = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { CheckUser = true }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CheckUser1(string Username, Role role)
        {
            User CheckUser;

            CheckUser = UserRepo.FilterUserName(Username, role);

            if (CheckUser != null)
            {
                var text = "This Username Is Exist For This Roles, Please Try Another Username Or Roles";
                return Json(new { text, CheckUser = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { CheckUser = true }, JsonRequestBehavior.AllowGet);
            }
        }
        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Username,Password,Roles")] User user)
        {
            if (ModelState.IsValid)
            {
                UserRepo.UpdateUser(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {

            var checkAdmin = CheckUser();
            ViewBag.Name = checkAdmin.Username;
            User user = UserRepo.GetUser(id);
            //var Check = CheckDelete(id);
            //if (Check == false)
            //{
            //    ViewBag.error = "This User Is In Onordering\n Are You Sure Delete Is User Now ?";
            //}
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }
        public ActionResult CheckDelete(int? id)
        {
            var FindTable = TableRepo.GetUserTable(id);
            var CheckOrder = CartRepo.GetTableCart(FindTable.TableId);
            if (CheckOrder.Count() != 0)
            {
                var text = "This User Is Inorderring, Are You Sure You Want To Delete This User ?";
                return Json(new { text, CheckUser = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { CheckUser = true }, JsonRequestBehavior.AllowGet);
            }
        }
        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = UserRepo.GetUser(id);
            var FindTable = TableRepo.GetUserTable(id);
            var CheckOrder = CartRepo.GetTableCart(FindTable.TableId);
            if (CheckOrder.Count() == 0)
            {
                CartRepo.RemoveCartList(FindTable.TableId);
            }
            if (FindTable != null)
            {

                FindTable.TableStatus = TableStatus.Empty;
                FindTable.TotalPrice = 0;
                FindTable.TotalQuantity = 0;
                FindTable.UserId = null;
                TableRepo.UpdateTable(FindTable);

            }
            UserRepo.RemoveUser(user);
            return RedirectToAction("Index");
        }

        public ActionResult ListTable()
        {

            var checkAdmin = CheckUser();
            ViewBag.Name = checkAdmin.Username;
            return View(TableRepo.GetTables());
        }

        public ActionResult CreateTable()
        {

            var checkAdmin = CheckUser();
            ViewBag.Name = checkAdmin.Username;
            ViewBag.Name = checkAdmin.Username;
            return View();
        }

        [HttpPost]
        public ActionResult CreateTable([Bind(Include = "TableId,TableNo,TableStatus")] Table table)
        {
            table.TableStatus = TableStatus.Empty;
            TableRepo.AddTable(table);
            return View("ListTable");
        }

        public ActionResult Logout(string Username)
        {
            if (Username == "Admin")
            {
                ViewBag.GetLogout = "Admin";
            }
            else if (Username == "Cashier")
            {
                ViewBag.GetLogout = "Cashier";
            }
            else if (Username == "Customers")
            {
                ViewBag.GetLogout = "Customers";
            }
            Session.Abandon();
            return View();
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
