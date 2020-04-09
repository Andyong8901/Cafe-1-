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
using Cafe.Web.Repository;

namespace Cafe.Web.Controllers
{
    public class AdminController : Controller
    {
        private CreateDB db = new CreateDB();
        //UserRepository UserRepo = new UserRepository();
        public void PreloadData()
        {
            List<User> admins = new List<User>()
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
            if (db.Users.Count() == 0)
            {
                db.Users.AddRange(admins);
                db.SaveChanges();
            }

            if (db.Tables.Count() == 0)
            {
                db.Tables.AddRange(tables);
                db.SaveChanges();
            }
        }
        public ActionResult Login()
        {
            PreloadData();
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginVM loginVM)
        {
            //var admin = GetUser();
            var Admin = db.Users.SingleOrDefault(a => a.Username == loginVM.Username && a.Roles == Role.Admin);
            if (Admin != null)
            {
                if (loginVM.Password != Admin.Password)
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

        // GET: Admin
        public ActionResult Index()
        {
            var Id = Convert.ToInt32(Session["AdminId"]);

            var checkAdmin = db.Users.SingleOrDefault(u => u.UserId == Id);
            if (checkAdmin == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Name = checkAdmin.Username;
            return View(db.Users.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            var Id = Convert.ToInt32(Session["AdminId"]);
            var checkAdmin = db.Users.SingleOrDefault(u => u.UserId == Id);
            if (checkAdmin == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Name = checkAdmin.Username;
            User user = db.Users.Find(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            var Id = Convert.ToInt32(Session["AdminId"]);
            var checkAdmin = db.Users.SingleOrDefault(u => u.UserId == Id);
            if (checkAdmin == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Name = checkAdmin.Username;
            return View();
        }
        public ActionResult CheckUser(string Username, Role role, int? Id)
        {
            List<User> FilterUser = new List<User>();
            User CheckUser = new User();
            var FindEdit = db.Users.SingleOrDefault(u => u.UserId == Id);
            if (FindEdit != null)
            {
                FilterUser = db.Users.Where(u => u.UserId != FindEdit.UserId).ToList();
                CheckUser = FilterUser.SingleOrDefault(f => f.Username == Username && f.Roles == role);
            }
            else
            {
                CheckUser = db.Users.SingleOrDefault(u => u.Username == Username && u.Roles == role);
            }

            if ((FilterUser.Count() == 0 && CheckUser != null) || (FilterUser.Count() != 0 && CheckUser != null))
            {
                var text = "This Username Is Exist For This Roles, Please Try Another Username Or Roles";
                return Json(new { text, CheckUser = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { CheckUser = true }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Username,Password,Roles")] User user)
        {
            var CheckinListUser = db.Users.SingleOrDefault(c => c.Username == user.Username && c.Roles == user.Roles);
            if (ModelState.IsValid)
            {
                if (CheckinListUser != null)
                {
                    ViewBag.Finded = "Cannot Use This Username Please Try Another";
                }
                else
                {
                    db.Users.Add(user);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            var Id = Convert.ToInt32(Session["AdminId"]);
            var checkAdmin = db.Users.SingleOrDefault(u => u.UserId == Id);
            if (checkAdmin == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Name = checkAdmin.Username;
            User user = db.Users.Find(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Username,Password,Roles")] User user)
        {
            var NowEditUser = db.Users.SingleOrDefault(u => u.UserId == user.UserId);
            var FilterName = db.Users.Where(c => c.Username != NowEditUser.Username).ToList();
            var CheckinListUser = FilterName.SingleOrDefault(c => c.Username == user.Username && c.Roles == user.Roles);
            if (ModelState.IsValid)
            {
                if (CheckinListUser != null)
                {
                    ViewBag.Finded = "Cannot Use This Username Please Try Another";
                }
                else
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            var Id = Convert.ToInt32(Session["AdminId"]);
            var checkAdmin = db.Users.SingleOrDefault(u => u.UserId == Id);
            if (checkAdmin == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Name = checkAdmin.Username;
            User user = db.Users.Find(id);
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
            var FindTable = db.Tables.SingleOrDefault(t => t.UserId == id);
            var CheckOrder = db.OrderCarts.Where(o => o.TableId == FindTable.TableId).ToList();
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
            User user = db.Users.Find(id);
            var CheckTable = db.Tables.SingleOrDefault(t => t.UserId == id);
            var CheckOrder = db.OrderCarts.Where(o => o.TableId == CheckTable.TableId).ToList();
            if (CheckOrder.Count() == 0)
            {
                db.OrderCarts.RemoveRange(CheckOrder);
            }
            if (CheckTable != null)
            {
                Table table = new Table()
                {
                    TableStatus = TableStatus.Empty,
                    TotalPrice = 0,
                    TotalQuantity = 0,
                    UserId = null,
                };
            }
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ListTable()
        {
            var Id = Convert.ToInt32(Session["AdminId"]);
            var checkAdmin = db.Users.SingleOrDefault(u => u.UserId == Id);
            if (checkAdmin == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Name = checkAdmin.Username;
            return View(db.Tables.ToList());
        }

        public ActionResult CreateTable()
        {
            var Id = Convert.ToInt32(Session["AdminId"]);
            var checkAdmin = db.Users.SingleOrDefault(u => u.UserId == Id);
            if (checkAdmin == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Name = checkAdmin.Username;
            return View();
        }

        [HttpPost]
        public ActionResult CreateTable([Bind(Include = "TableId,TableNo,TableStatus")] Table table)
        {
            table.TableStatus = TableStatus.Empty;
            db.Tables.Add(table);
            db.SaveChanges();
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
