using ShoppingCart.Data.DML.Model;
using System;
using System.Threading.Tasks;

namespace ShoppingCart.Data.DAL.Data.Interfaces
{
    public interface IShoppingCartRepository : IBaseRepository<CustomerShoppingCart>
    {
        Task<CustomerShoppingCart> GetCustomerShoppingCart(int customerId);
        Task<CustomerShoppingCart> PushCartItem(ShoppingCartItem cartItem, int customerId, Guid id);
        Task<bool> DeleteCartItem(ShoppingCartItem cartItem, int customerId, Guid id);
        Task<bool> AddAsyncIfNotExists(CustomerShoppingCart cart);
    }
}
