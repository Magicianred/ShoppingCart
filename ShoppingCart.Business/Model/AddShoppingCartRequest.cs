using ShoppingCart.Data.DML.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Business.Model
{
    public class ShoppingCartUpdateRequest :ShoppingCartItem
    {
        public Customer Customer { get; set; }
    }
}
