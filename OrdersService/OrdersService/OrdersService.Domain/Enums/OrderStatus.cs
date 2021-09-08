using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Domain.Enums
{
    public enum OrderStatus
    {
        OrderCreated = 1,
        OrderRecieved = 2,
        OrderRecievedAndCooking = 3,
        OrderRecievedCookedAndDispatched = 4,
        OrderDelievered = 5
    }
}
