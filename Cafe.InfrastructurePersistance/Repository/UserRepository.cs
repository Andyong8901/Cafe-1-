using Cafe.DomainModelEntity;
using Cafe.InfrastructurePersistance;
using Cafe.Web.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using static Cafe.DomainModelEntity.User;

namespace Cafe.InfrastructurePersistance.Repository
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
            var finded = db.Users.Find(id);
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

        public User CheckUser(User user)
        {
            var GetUser = db.Users.SingleOrDefault(a => a.Username == user.Username && a.Roles == user.Roles);
            return GetUser;
        }


        public User FilterUser(string Username, Role role, int? Id)
        {
            var FilterUser = db.Users.Where(u => u.UserId != Id).ToList(); 
            var CheckUser = FilterUser.SingleOrDefault(f => f.Username == Username && f.Roles == role);
            return CheckUser;
        }

        public void AddUserList(List<User> users)
        {
            db.Users.AddRange(users);
            Save();
        }
    }
}