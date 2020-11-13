using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ShoppingCart.Data.DAL.Data.Interfaces;
using ShoppingCart.Data.DAL.Helpers;
using ShoppingCart.Data.DML.Model;
using System.Threading.Tasks;

namespace ShoppingCart.Data.DAL.Data
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(IOptions<MongoDbSettings> options) : base(options)
        {
        }
        public async Task<Product> GetProductInfo(string barcode)
        {
            return await this.Context.Collection.Find<Product>(i => i.Barcode == barcode).FirstOrDefaultAsync();
        }

        public virtual async Task<bool> AddAsyncIfNotExists(Product cart)
        {
            var options = new InsertOneOptions { BypassDocumentValidation = false };

            var result = await Context.Collection.ReplaceOneAsync(p => p.Barcode == cart.Barcode, cart
                , new ReplaceOptions { IsUpsert = true });

            return result.IsAcknowledged;
        }
    }
}
