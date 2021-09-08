using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Domain.ViewModels.Request
{
    public class ProductModel
    {
        public long ProductId { get; set; }

        public string ProductName { get; set; }
    }
}
