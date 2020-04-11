using Cafe.DomainModelEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cafe.Web.Interface
{
    public interface ITableRepository
    {
        IEnumerable<Table> GetTables();
        Table GetTable(int? id);
        void AddTable(Table table);
        void AddTableList(List<Table> table);
        void UpdateTable(Table table);
        void RemoveTable(Table table);
        Table CheckTableNo(string TableNo);
        IEnumerable<Table> CheckTableStatus();
        void Save();
    }
}