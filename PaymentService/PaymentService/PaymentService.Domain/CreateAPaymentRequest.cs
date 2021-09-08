using PaymentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentService.Domain
{
    public class CreateAPaymentRequest
    {
        public long OrderNumber { get; set; }

        public string PaymentMethod { get; set; }

        public UserModelRequest UserDetails { get; set; }
    }
}
