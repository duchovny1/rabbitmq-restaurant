using System;
using System.Collections.Generic;
using System.Text;

namespace Prices.Domain.ViewModels.Response
{
    public class ProductPriceResponse
    {
        public string ProductName { get; set; }

        public long ProductId { get; set; }

        public decimal PriceAmount { get; set; }
    }
}
