using Cafe.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cafe.Web.Interface
{
    public interface ITableRepository
    {
        IEnumerable<Table> GetUsers();
        Table GetUser(int? id);
        void AddUser(Table user);
        void UpdateUser(Table user);
        void RemoveUser(Table user);
        void Save();
    }
}