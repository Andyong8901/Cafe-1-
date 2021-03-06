﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cafe.DomainModelEntity
{
    public class Table
    {
        [Key]
        public int TableId { get; set; }
        public string TableNo { get; set; }
        public TableStatus TableStatus { get; set; }
        public double TotalPrice { get; set; }
        public int TotalQuantity { get; set; }


        public int? UserId { get; set; }
        public User User { get; set; }

        public virtual ICollection<OrderCart> OrderCarts { get; set; }
    }
    public enum TableStatus
    {
        Empty, Occupied
    }
}