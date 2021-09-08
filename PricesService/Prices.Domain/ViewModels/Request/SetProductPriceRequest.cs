using System;
using System.Collections.Generic;
using System.Text;

namespace Prices.Domain.ViewModels.Request
{
    public class SetProductPriceRequest
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
