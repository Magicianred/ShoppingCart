using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ShoppingCart.Data.DAL.Data.Interfaces;
using ShoppingCart.Data.DAL.Helpers;
using ShoppingCart.Data.DML.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Data.DAL.Data
{
    public class ShoppingCartRepository : BaseRepository<CustomerShoppingCart>,  IShoppingCartRepository
    {
        public ShoppingCartRepository(IOptions<MongoDbSettings> options) : base(options)
        {

        }

        public async Task<CustomerShoppingCart> GetCustomerShoppingCart(int customerId)
        {
            return await this.Context.Collection.Find(x => x.CustomerId == customerId).FirstOrDefaultAsync();
        }

        public async Task<CustomerShoppingCart> PushCartItem(ShoppingCartItem cartItem , int customerId, Guid id)
        {
            var result = await this.Context.Collection.FindOneAndUpdateAsync(
                Builders<CustomerShoppingCart>.Filter.Eq(x => x.CustomerId , customerId),
                Builders<CustomerShoppingCart>.Update.Push(x => x.ShoppingCartItems, cartItem));
            return result;
        }

        public virtual async Task<bool> AddAsyncIfNotExists(CustomerShoppingCart cart)
        {
            var options = new InsertOneOptions { BypassDocumentValidation = false };

            var result = await Context.Collection.ReplaceOneAsync(p => p.CustomerId == cart.CustomerId,cart
                ,new ReplaceOptions { IsUpsert = true });

            return result.IsAcknowledged;
        }

        public async Task<bool> DeleteCartItem (ShoppingCartItem cartItem, int customerId, Guid id)
        {
            var filter = Builders<CustomerShoppingCart>.Filter.Eq(item => item.CustomerId, customerId);
            var update = Builders<CustomerShoppingCart>.Update.PullFilter(i=>i.ShoppingCartItems,
                                        Builders<ShoppingCartItem>.Filter.Eq(x => x.ProductItem.Barcode, cartItem.ProductItem.Barcode));

            UpdateResult updateResult = await this.Context.Collection.UpdateOneAsync(filter, update);

            return updateResult.IsAcknowledged;
        }
    }
}
