using ShoppingCart.Business.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using ShoppingCart.Data.DML.Model;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using ShoppingCart.Data.DAL.Data.Interfaces;

namespace ShoppingCart.Business.Service
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IServiceProvider _provider;
        public ShoppingCartService(IServiceProvider provider)
        {
            _provider = provider;
        }
        //By clicking the shopping cart on wb site 
        public async Task<ShoppingCartServiceResponse> Get(ShoppingCartServiceRequest request)
        {
            ShoppingCartServiceResponse response = new ShoppingCartServiceResponse { CustomerId = request.CustomerId };
            var shoppingCart = await _provider.GetService<IShoppingCartRepository>().GetCustomerShoppingCart(request.CustomerId);

            if (shoppingCart != null)
            {
                foreach (var basketItem in shoppingCart.ShoppingCartItems)
                {
                    var product = await GetProduct(basketItem.ProductItem.Barcode);
                    var totalbasketItemCount = shoppingCart.ShoppingCartItems.Where(i => i.ProductItem.Barcode == basketItem.ProductItem.Barcode).Sum(i => i.ItemBasketQuantity);
                    if (basketItem.ProductItem.Price != product.Price || totalbasketItemCount > product.StockQuantity)
                    {
                        shoppingCart = await UpdateBasketItemInfo(product, shoppingCart);
                    }
                }
                response.ShoppingCartItems = shoppingCart.ShoppingCartItems;
                response.Id = shoppingCart.Id;
                response.CreateDate = shoppingCart.CreateDate;
                response.CustomerId = shoppingCart.CustomerId;
            }
            return response;
        }
        //Basket action- Can be called for all operations-changes whhile the shopping cart screen is open. increase item count,decrease item count etc.
        public async Task<bool> Update(ShoppingCartUpdateRequest request)
        {
            CustomerShoppingCart shoppingCart = new CustomerShoppingCart();
            var item = await GetProduct(request.ProductItem.Barcode);

            var existingCart = await Get(new ShoppingCartServiceRequest { CustomerId = request.Customer.CustomerId });

            ShoppingCartItem cartItem = new ShoppingCartItem
            {
                GiftPrice = request.GiftPrice,
                ItemBasketQuantity = request.ItemBasketQuantity,
                ProductItem = item
            };
    
            //müşterinin sepeti yoksa sepet oluştur
            if (existingCart.ShoppingCartItems == null)
            {
                return await CreateShoppingCart(cartItem, request.Customer.CustomerId);
            }

            int basketExistingItemCount = existingCart.ShoppingCartItems.Where(i => i.ProductItem.Barcode == request.ProductItem.Barcode).Sum(i => i.ItemBasketQuantity);
            if (item == null || item.StockQuantity < request.ItemBasketQuantity)
                return false;

            //ürün sepette yoksa ve req edlen toplamı toplam ürün stoğunu geçmiyorsa
            if (basketExistingItemCount == 0)
            {
                shoppingCart = await _provider
                    .GetService<IShoppingCartRepository>()
                    .PushCartItem(cartItem, request.Customer.CustomerId, Guid.NewGuid());
            }

            //ürün sepette varsa stoğu güncellensin            
            else
            {
                existingCart.ShoppingCartItems.Where(i => i.ProductItem.Barcode == request.ProductItem.Barcode).ToList().ForEach(i => i.ItemBasketQuantity = request.ItemBasketQuantity);
                shoppingCart = await UpdateBasketItemInfo(item, existingCart);
            }
            return shoppingCart.ShoppingCartItems != null ? true : false;
        }
        //Basket action- can be applied with a trashbin icon in basket next to item
        public async Task<bool> Delete(ShoppingCartUpdateRequest request)
        {
            var item = await GetProduct(request.ProductItem.Barcode);
            var existingCart = await Get(new ShoppingCartServiceRequest { CustomerId = request.Customer.CustomerId });
            int basketExistingItemCount = existingCart.ShoppingCartItems.Where(i => i.ProductItem.Barcode == request.ProductItem.Barcode).Sum(i => i.ItemBasketQuantity);

            ShoppingCartItem cartItem = new ShoppingCartItem
            {
                GiftPrice = request.GiftPrice,
                ItemBasketQuantity = request.ItemBasketQuantity,
                ProductItem = request.ProductItem
            };
            var result = await _provider.GetService<IShoppingCartRepository>().DeleteCartItem(cartItem,request.Customer.CustomerId,existingCart.Id);
            return result;
        }


        #region Service Helper
        public async Task<bool> CreateShoppingCart(ShoppingCartItem cartItem, int customerId)
        {
            CustomerShoppingCart shoppingCart = new CustomerShoppingCart { CreateDate = DateTime.Now, CustomerId = customerId, ShoppingCartItems = new List<ShoppingCartItem> { cartItem } };
            return await _provider.GetService<IShoppingCartRepository>().AddAsyncIfNotExists(shoppingCart);
        }

        public async Task<CustomerShoppingCart> UpdateBasketItemInfo(Product product, CustomerShoppingCart shoppingCart)
        {
            var existingBasketItem = shoppingCart.ShoppingCartItems.Where(i => i.ProductItem.Barcode == product.Barcode).ToList();
            existingBasketItem.ForEach(i => i.ProductItem = product);
            if (product.StockQuantity < existingBasketItem.Sum(i => i.ItemBasketQuantity))
            {
                existingBasketItem.ForEach(i => i.ItemBasketQuantity = product.StockQuantity);
            }
            return await _provider.GetService<IShoppingCartRepository>().UpdateAsync(shoppingCart.Id, shoppingCart);
        }

        #endregion

        #region Product
        public async Task<bool> AddProduct(Product product)
        {
            product.CreateDate = DateTime.Now;
            return await _provider.GetService<IProductRepository>().AddAsyncIfNotExists(product);
        }

        public async Task<Product> GetProduct(string barcode)
        {
            return await _provider.GetService<IProductRepository>().GetProductInfo(barcode);
        }
        #endregion
    }
}
