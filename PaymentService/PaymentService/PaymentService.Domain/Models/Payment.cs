using PaymentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentService.Domain.Models
{
    public class Payment
    {
        public Payment()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }

        public long OrderNumber { get; set; }

        public bool IsBeenProccessed { get; set; }

        public decimal Price { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
