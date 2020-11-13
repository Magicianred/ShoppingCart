using ShoppingCart.Data.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.DML.Model
{
    public class ShoppingCartItem
    {
        public Product ProductItem { get; set; }
        public int ItemBasketQuantity { get; set; }
        public decimal GiftPrice { get; set; }
    }

    public class CustomerShoppingCart: BaseEntity
    {
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
        public int CustomerId { get; set; }
    }
}
