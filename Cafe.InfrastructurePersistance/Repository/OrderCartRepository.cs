using Cafe.DomainModelEntity;
using Cafe.DomainModelEntity.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.InfrastructurePersistance.Repository
{
    public class OrderCartRepository : IOrderCartRepository
    {
        private CreateDB db = new CreateDB();
        public void AddOrderCart(OrderCart orderCart)
        {
            db.OrderCarts.Add(orderCart);
            Save();
        }

        public OrderCart GetOrderCart(int? id)
        {
            var GetOrder = db.OrderCarts.SingleOrDefault(o => o.OrdercartId == id);
            return GetOrder;
        }

        public IEnumerable<OrderCart> GetTableCart(int? Id)
        {
            var CartList = db.OrderCarts.Where(o => o.TableId == Id).ToList();
            return CartList;
        }

        public void RemoveOrderCart(OrderCart orderCart)
        {
            db.OrderCarts.Remove(orderCart);
            Save();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void UpdateOrderCart(OrderCart orderCart)
        {
            db.Entry(orderCart).State = EntityState.Modified;
            Save();
        }

        public void RemoveCartList(int Id)
        {
            var CheckOrder = db.OrderCarts.Where(o => o.TableId == Id).ToList();
            db.OrderCarts.RemoveRange(CheckOrder);
            Save();
        }
        public OrderCart FindCategoryInCart(int id,int tableId)
        {
            var FindCartCategory = db.OrderCarts.SingleOrDefault(c => c.CategoriesId == id && c.TableId == tableId);
            return FindCartCategory;
        }
    }
}
