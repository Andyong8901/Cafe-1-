using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cafe.Web.Models
{
    public class Categories
    {
        public int CategoriesId { get; set; }
        public Category CategoryName { get; set; }
        public byte[] FoodImg { get; set; }
        public string FoodName { get; set; }
        public double UnitPrice { get; set; }
        public string Remark { get; set; }

        public virtual ICollection<OrderCart> OrderCarts { get; set; }

        public enum Category
        {
            Rice, Soup, Drinks, Dessert
        }
    }
}