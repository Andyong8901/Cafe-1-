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
    public class CustomersController : Controller
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
            Users.Roles = Role.Customer;
            var customer = UserRepo.CheckUser(Users);


            if (Users.Password != customer.Password)
            {
                ViewBag.error = "Invalid Username Or Password \nPlease Try Again";
                return View();
            }
            else
            {
                Session["customerId"] = customer.UserId;
                return RedirectToAction("SelectTable");
            }
        }
        public User CheckUser()
        {
            var CustomerId = Convert.ToInt32(Session["customerId"]);
            var CheckLogin = UserRepo.GetUser(CustomerId);
            
            return CheckLogin;
        }

        // GET: Customers
        public ActionResult SelectTable()
        {
           var user =  CheckUser();
            var CheckTable = TableRepo.GetUserTable(user.UserId);
            if (CheckTable != null)
            {
                return RedirectToAction("Menu");
            }
            else
            {
                var ListTable = TableRepo.CheckTableStatus();
                return View(ListTable);
            }
        }

        public ActionResult SelectedTable(int? id)
        {

            var user = CheckUser();
            var ListTable = TableRepo.GetTable(id);
            if (ListTable != null)
            {
                ListTable.TableStatus = TableStatus.Occupied;
                ListTable.UserId = user.UserId;
                TableRepo.UpdateTable(ListTable);
                return RedirectToAction("Menu");
            }
            return View();
        }

        public ActionResult Menu()
        {
            var user = CheckUser();
            var UserTable = TableRepo.GetUserTable(user.UserId);
            if (UserTable.OrderCarts.Count() != 0)
            {
                Session["TotalQuantity"] = LoopItem(UserTable);
            }
            else
            {
                Session["TotalQuantity"] = 0;
            }
            ViewBag.Name = user.Username;
            ViewBag.TableNo = UserTable.TableNo;
            return View(CategoryRepo.GetCategories());
        }

        public ActionResult AddItem(int? id)
        {

            var user = CheckUser();
            var Item = CategoryRepo.GetCategory(id);
            var CheckTable = TableRepo.GetUserTable(user.UserId);
            var CheckItem = CartRepo.FindCategoryInCart(Item.CategoriesId, CheckTable.TableId);

            if (CheckItem != null)
            {
                CheckItem.Quantity++;
                CheckItem.TotalAmount = Item.UnitPrice * CheckItem.Quantity;
                CartRepo.UpdateOrderCart(CheckItem);
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
                CartRepo.AddOrderCart(ordercart);
            }
            return RedirectToAction("Menu");
        }

        public ActionResult ListCart()
        {

            var user = CheckUser();
            var CheckTable = TableRepo.GetUserTable(user.UserId);
            ViewBag.TableId = CheckTable.TableId;
            Session["TotalQuantity"] = LoopItem(CheckTable);
            var CustomerCart = CartRepo.GetTableCart(CheckTable.TableId);
            ViewBag.TotalAll = CheckTable.TotalPrice;
            return View(CustomerCart);
        }
        public ActionResult CancelItem(int? id)
        {
            CheckUser();
            var getItem = CartRepo.GetOrderCart(id);
            CartRepo.RemoveOrderCart(getItem);
            return RedirectToAction("ListCart");
        }

        public ActionResult ClearAllItem()
        {

            var user = CheckUser();
            var CheckTable = TableRepo.GetUserTable(user.UserId);
            CartRepo.RemoveCartList(CheckTable.TableId);
            return RedirectToAction("Menu");
        }
        public ActionResult PlusItem(int? id)
        {
            var CheckItem = CartRepo.GetOrderCart(id);

            CheckItem.Quantity++;
            CheckItem.TotalAmount = CheckItem.Categories.UnitPrice * CheckItem.Quantity;
            CartRepo.UpdateOrderCart(CheckItem);

            return RedirectToAction("ListCart");
        }

        public ActionResult MinusItem(int? id)
        {
            var CheckItem = CartRepo.GetOrderCart(id);
            CheckItem.Quantity--;
            if (CheckItem.Quantity == 0)
            {
                CartRepo.RemoveOrderCart(CheckItem);
                return RedirectToAction("ListCart");
            }
            CheckItem.TotalAmount = CheckItem.Categories.UnitPrice * CheckItem.Quantity;
            CartRepo.UpdateOrderCart(CheckItem);
            return RedirectToAction("ListCart");
        }


        public int LoopItem(Table UserTable)
        {
            var TotalQuan = 0;
            var totalitem = CartRepo.GetTableCart(UserTable.TableId);

            double TotalAmount = 0;
            foreach (var item in totalitem)
            {
                TotalQuan += item.Quantity;
                TotalAmount += item.Quantity * item.Categories.UnitPrice;
            }
            var Table = TableRepo.GetTable(UserTable.TableId);
            Table.TotalQuantity = TotalQuan;
            Table.TotalPrice = TotalAmount;
            TableRepo.UpdateTable(Table);
            return TotalQuan;

        }

        public ActionResult ConfirmOrder(int? id)
        {
            var user = CheckUser();
            var CheckTable = TableRepo.GetUserTable(user.UserId);
            ViewBag.TableNo = CheckTable.TableNo;
            ViewBag.TotalAllPrice = CheckTable.TotalPrice;
            var ListUserCart = CartRepo.GetTableCart(id);
            return View(ListUserCart);
        }

        [HttpPost]
        public ActionResult ConfirmOrder()
        {
            var user = CheckUser();
            var CheckTable = TableRepo.GetUserTable(user.UserId);
            CartRepo.RemoveCartList(CheckTable.TableId);
            CheckTable.TableStatus = TableStatus.Empty;
            CheckTable.TotalQuantity = 0;
            CheckTable.TotalPrice = 0;
            CheckTable.UserId = null;
            TableRepo.UpdateTable(CheckTable);
            return RedirectToAction("Logout", "Admin", new { UserName = "Customers" });
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
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
