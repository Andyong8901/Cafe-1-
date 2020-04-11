using Cafe.DomainModelEntity;
using Cafe.InfrastructurePersistance;
using Cafe.Web.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Cafe.InfrastructurePersistance.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private CreateDB db = new CreateDB();

        public void AddCategory(Categories categories)
        {
            db.Categories.Add(categories);
            db.SaveChanges();
        }

        public IEnumerable<Categories> GetCategories()
        {
            var ListCategories = db.Categories.ToList();
            return ListCategories;
        }

        public Categories GetCategory(int? id)
        {
           var GetGategory = db.Categories.SingleOrDefault(c => c.CategoriesId == id);
            return GetGategory;
        }

        public void RemoveCategory(Categories categories)
        {
            db.Categories.Remove(categories);
            Save();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void UpdateCategory(Categories categories)
        {
            db.Entry(categories).State = EntityState.Modified;
            Save();
        }
    }
}