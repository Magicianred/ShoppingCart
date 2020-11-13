using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.CompilerServices;
using ShoppingCart.Business.Service;
using ShoppingCart.Data.DAL.Data.Interfaces;
using ShoppingCart.Data.DML.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Test
{
    public class GenerateDummy
    {
        private readonly IServiceProvider _provider;

        public GenerateDummy(IServiceProvider provider)
        {
            _provider = provider;
        }
        public List<Product> GenerateDummyProductList ()
        {
            List<Product> productList = new List<Product> {
            new Product { Barcode = "897766474745784", SKU = "ABC-123" , StockQuantity = 5 , Price = 123.5M },
            new Product { Barcode = "897766474745785", SKU = "ABC-122" , StockQuantity = 4 , Price = 120.5M },
            new Product { Barcode = "897766474745783", SKU = "ABC-121" , StockQuantity = 7 , Price = 80.5M },
            new Product { Barcode = "897766474745784", SKU = "ABC-123" , StockQuantity = 0 , Price = 45M }
            };
            productList.ForEach(i => i.CreateDate = DateTime.Now);

            return productList;
        }
    }
}
