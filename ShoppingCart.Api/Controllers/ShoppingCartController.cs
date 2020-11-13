using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Business.Model;
using ShoppingCart.Business.Service;
using ShoppingCart.Data.DML.Model;

namespace ShoppingCart.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IServiceProvider _provider;
        public ShoppingCartController(IServiceProvider provider)
        {
            this._provider = provider;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<CustomerShoppingCart> Get(Guid id, int customerId)
        {
            ShoppingCartServiceRequest request = new ShoppingCartServiceRequest { CustomerId = customerId, Id = id };
            using var scope = _provider.CreateScope();
            return await _provider.GetService<IShoppingCartService>().Get(request);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<bool> Update([FromBody] ShoppingCartUpdateRequest request)
        {
            using (var scope = _provider.CreateScope())
            {
                return await _provider.GetService<IShoppingCartService>().Update(request);
            }
        }

        [HttpPost]
        [Route("Remove")]
        public async Task<bool> Remove([FromBody] ShoppingCartUpdateRequest request)
        {
            using (var scope = _provider.CreateScope())
            {
                return await _provider.GetService<IShoppingCartService>().Delete(request);
            }
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<bool> AddProduct([FromBody] Product product)
        {
            using (var scope = _provider.CreateScope())
            {
                return await _provider.GetService<IShoppingCartService>().AddProduct(product);
            }
        }
    }
}