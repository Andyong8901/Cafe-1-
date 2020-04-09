using Cafe.Web.Models;
using Cafe.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cafe.Web.Interface
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetUser(int? id);
        void AddUser(User user);
        void UpdateUser(User user);
        void RemoveUser(User user);
        void Save();
    }
}