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
    public class AdminController : Controller
    {
        private CreateDB db = new CreateDB();
        public void CreateAdmin()
        {
            List<User> admins = new List<User>()
            {
                new User(){Username = "John",Password="123456",Roles=Role.Admin }
            };
            if (db.Users.Count() == 0)
            {
                db.Users.AddRange(admins);
                db.SaveChanges();
            }
            else
            {
                return;
            }
        }
        public ActionResult Login()
        {
            CreateAdmin();
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginVM loginVM)
        {
            var Admin = db.Users.SingleOrDefault(a => a.Username == loginVM.Username && a.Password == loginVM.Password && a.Roles == Role.Admin);
            if (Admin != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
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
