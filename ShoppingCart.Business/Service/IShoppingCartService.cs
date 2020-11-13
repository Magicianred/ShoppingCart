using ShoppingCart.Business.Model;
using ShoppingCart.Data.DML.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Business.Service
{
    public interface IShoppingCartService
    {
        Task<bool> Update(ShoppingCartUpdateRequest request);
        Task<bool> AddProduct(Product product);
        Task<ShoppingCartServiceResponse> Get(ShoppingCartServiceRequest request);
        Task<bool> Delete(ShoppingCartUpdateRequest request);
    }
}
