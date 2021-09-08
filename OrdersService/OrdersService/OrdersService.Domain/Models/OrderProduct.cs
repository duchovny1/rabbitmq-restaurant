using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Domain.Models
{
    public class OrderProduct
    {
        public long Id { get; set; }

        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public string OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}
