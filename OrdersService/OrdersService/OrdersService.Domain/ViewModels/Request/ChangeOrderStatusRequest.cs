using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Domain.ViewModels.Request
{
    public class ChangeOrderStatusRequest
    {
        public long OrderNumber { get; set; }
        public string OrderStatus { get; set; }
    }
}
