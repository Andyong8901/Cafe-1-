using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cafe.Web.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Roles { get; set; }


        public virtual ICollection<Table> Tables { get; set; }
        public enum Role
        {
            Admin, Customer, Cashers
        }
    }
}