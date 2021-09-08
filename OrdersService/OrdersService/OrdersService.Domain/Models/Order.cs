using OrdersService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Domain.Models
{
    public class Order
    {
        public Order()
        {
            Products = new HashSet<OrderProduct>();
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public long OrderNumber { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public bool IsPaid { get; set; }
        public ICollection<OrderProduct> Products { get; set; }
    }
}
