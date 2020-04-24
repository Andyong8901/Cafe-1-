using Cafe.DomainModelEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Cafe.DomainModelEntity.User;

namespace Cafe.Web.Interface
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetUser(int? id);
        void AddUser(User user);
        void AddUserList(List<User> users);
        void UpdateUser(User user);
        void RemoveUser(User user);
        void Save();
        User CheckUser(User user);
        User FilterUser(string Username, Role role, int? Id);
        User FilterUserName(string Username, Role role);
    }
}