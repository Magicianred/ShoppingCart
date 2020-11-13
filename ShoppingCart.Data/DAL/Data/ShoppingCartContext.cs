using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ShoppingCart.Data.DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.DAL.Data
{
    public class ShoppingCartContext<T>
    {
        private readonly IMongoDatabase _database = null;
        private readonly MongoDbSettings settings;

        public ShoppingCartContext(IOptions<MongoDbSettings> options)
        {
            this.settings = options.Value;
            var client = new MongoClient(this.settings.ConnectionString);
            _database = client.GetDatabase(this.settings.DatabaseName);
        }

        public IMongoCollection<T> Collection
        {
            get
            {
                return _database.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
            }
        }
    }
}
