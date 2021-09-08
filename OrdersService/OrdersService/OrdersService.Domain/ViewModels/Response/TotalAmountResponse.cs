using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Domain.ViewModels.Response
{
    public class TotalAmountResponse
    {
        public TotalAmountResponse(long orderNumber, decimal totalAmount)
        {
            OrderNumber = orderNumber;
            TotalAmount = totalAmount;
        }
        public long OrderNumber { get; set; }

        public decimal TotalAmount { get; set; }

        public IEnumerable<string> Errors { get; set; } 
    }
}
