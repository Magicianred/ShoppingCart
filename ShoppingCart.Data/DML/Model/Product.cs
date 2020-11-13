using ShoppingCart.Data.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.DML.Model
{
    public class Product: BaseEntity
    {
        public string Barcode { get; set; }
        public string SKU { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
    }
}
