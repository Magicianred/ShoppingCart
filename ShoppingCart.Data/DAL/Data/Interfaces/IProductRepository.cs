using ShoppingCart.Data.DML.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.DAL.Data.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product> GetProductInfo(string barcode);
        Task<bool> AddAsyncIfNotExists(Product cart);
    }
}
