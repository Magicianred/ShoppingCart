using ShoppingCart.Data.DML.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Business.Model
{
    public class ShoppingCartServiceRequest
    {
        public Guid Id { get; set; }
        public int CustomerId { get; set; }
    }
}
