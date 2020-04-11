using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cafe.DomainModelEntity
{
    public class OrderCart
    {
        public int OrdercartId { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }

        public int CategoriesId { get; set; }
        public virtual Categories Categories { get; set; }

        public int? TableId { get; set; }
        public virtual Table Table { get; set; }

    }
}