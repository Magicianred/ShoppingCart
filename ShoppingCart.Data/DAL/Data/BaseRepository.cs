using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ShoppingCart.Data.DAL.Data.Interfaces;
using ShoppingCart.Data.DAL.Entities;
using ShoppingCart.Data.DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCart.Data.DAL.Data
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly ShoppingCartContext<T> Context = null;

        protected BaseRepository(IOptions<MongoDbSettings> options)
        {
            Context = new ShoppingCartContext<T>(options);
        }

        public virtual Task<T> GetByIdAsync(Guid id)
        {
            return Context.Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
        public virtual async Task<T> AddAsync(T entity)
        {
            var options = new InsertOneOptions { BypassDocumentValidation = false  };
            await Context.Collection.InsertOneAsync(entity, options);
            return entity;
        }
        public virtual async Task<bool> BulkAddAsync(List<T> entity)
        {
            var options = new BulkWriteOptions { BypassDocumentValidation = false };
            var listWrites = new List<WriteModel<T>>();
            foreach (var i in entity)
            {
                listWrites.Add(new InsertOneModel<T>(i));
            }
            var result = await Context.Collection.BulkWriteAsync(listWrites, options);
            return result.IsAcknowledged;
        }
        public virtual async Task<T> UpdateAsync(Guid id, T entity)
        {
            return await Context.Collection.FindOneAndReplaceAsync(x => x.Id == id, entity);
        }
    }
}
