using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.DomainModelEntity.Interface
{
    public interface IOrderCartRepository
    {
        IEnumerable<OrderCart> GetTableCart(int? Id);
        OrderCart GetOrderCart(int? id);
        void AddOrderCart(OrderCart orderCart);
        void UpdateOrderCart(OrderCart orderCart);
        void RemoveOrderCart(OrderCart orderCart);
        void RemoveCartList(int Id);
        OrderCart FindCategoryInCart(int id, int tableId);
        void Save();
    }
}
