using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OrdersService.Domain.ViewModels.Request
{
    public class OrdersInputModelRequest
    {
        public bool IsPaid { get; set; }
        public string OrderStatus { get; set; }
        public IEnumerable<ProductModelRequest> Products { get; set; }
    }
}
