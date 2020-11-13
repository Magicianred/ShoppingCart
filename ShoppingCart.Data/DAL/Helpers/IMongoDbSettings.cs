using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.DAL.Helpers
{
    public interface IMongoDbSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
}
