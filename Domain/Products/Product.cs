using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Products
{
    public partial class Product : BaseEntity<int>
    {
        public string Id { get; private set; }
        public int Stock { get; private set; }
        public decimal CurrentPrice { get; private set; }
        public decimal DefaultPrice { get; private set; }
    }
}
