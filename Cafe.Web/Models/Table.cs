using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cafe.Web.Models
{
    public class Table
    {
        public string TableId { get; set; }
        public string TableNo { get; set; }
        public TableStatus TableStutus { get; set; }
        public double TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
    }
    public enum TableStatus
    {
        Empty, Occupied
    }
}