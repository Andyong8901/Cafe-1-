using Cafe.Web.Interface;
using Cafe.Web.Models;
using Cafe.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Cafe.Web.Repository
{
    public class UserRepository : IUserRepository
    {
        private CreateDB db = new CreateDB();
        public IEnumerable<User> GetUsers()
        {
            return db.Users.ToList();
        }

        public User GetUser(int? id)
        {
            var finded = db.Users.SingleOrDefault(u => u.UserId == id);
            return finded;
        }

        public void AddUser(User user)
        {
            db.Users.Add(user);
            Save();
        }
        public void UpdateUser(User user)
        {
            db.Entry(user).State = EntityState.Modified;
            Save();
        }
        public void RemoveUser(User user)
        {
            db.Users.Remove(user);
            Save();
        }
        public void Save()
        {
            db.SaveChanges();
        }

    }
}