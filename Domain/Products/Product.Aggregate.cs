using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Products
{
    public partial class Product : IAggregateRoot
    {
        public Product(string id, int stock, decimal currentPrice, decimal defaultPrice)
        {
            Id = id;
            Stock = stock;
            CurrentPrice = currentPrice;
            DefaultPrice = defaultPrice;
        }

        public void SetProductStock(int stock)
        {
            Stock = stock;
        }
        public void SetProductCurrentPrice(decimal price)
        {
            CurrentPrice = price;
        }
    }
}
