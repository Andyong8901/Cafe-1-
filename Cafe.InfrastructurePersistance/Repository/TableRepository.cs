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
    public class TableRepository : ITableRepository
    {
        private CreateDB db = new CreateDB();

        public void AddTable(Table table)
        {
            db.Tables.Add(table);
            Save();
        }

        public Table GetTable(int? id)
        {
            var getTable = db.Tables.Find(id);
            return getTable;
        }
        public Table GetUserTable(int? id)
        {
            var GetUserTable = db.Tables.SingleOrDefault(t => t.UserId == id);
            return GetUserTable;
        }
        public IEnumerable<Table> GetTables()
        {
            var table = db.Tables.ToList();
            return table;
        }

        public void RemoveTable(Table table)
        {
            db.Tables.Remove(table);
            Save();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void UpdateTable(Table table)
        {
            db.Entry(table).State = EntityState.Modified;
            Save();
        }

        public void AddTableList(List<Table> table)
        {
            db.Tables.AddRange(table);
            Save();
        }

        public Table CheckTableNo(string TableNo)
        {
            var Checking = db.Tables.SingleOrDefault(T => T.TableNo == TableNo);
            return Checking;
        }
        public IEnumerable<Table> CheckTableStatus()
        {
            var filter = db.Tables.Where(t => t.TableStatus != TableStatus.Occupied).ToList();
            return filter;
        }
    }
}