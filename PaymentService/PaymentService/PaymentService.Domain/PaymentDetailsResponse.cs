using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentService.Domain
{
    public class PaymentDetailsResponse
    {
        public string Id { get; set; }

        public long OrderNumber { get; set; }

        public decimal TotalPrice { get; set; }

        public UserModelRequest UserDetails { get; set; }
    }
}
