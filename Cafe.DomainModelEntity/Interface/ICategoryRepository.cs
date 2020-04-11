using Cafe.DomainModelEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cafe.Web.Interface
{
    public interface ICategoryRepository
    {
        IEnumerable<Categories> GetCategories();
        Categories GetCategory(int? id);
        void AddCategory(Categories categories);
        void UpdateCategory(Categories categories);
        void RemoveCategory(Categories categories);
        void Save();
    }
}