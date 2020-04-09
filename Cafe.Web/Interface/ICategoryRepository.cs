using Cafe.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cafe.Web.Interface
{
    public interface ICategoryRepository
    {
        IEnumerable<Categories> GetUsers();
        Categories GetUser(int? id);
        void AddUser(Categories categories);
        void UpdateUser(Categories categories);
        void RemoveUser(Categories categories);
        void Save();
    }
}