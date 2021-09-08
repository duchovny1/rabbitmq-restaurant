using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentService.Domain
{
    public class PaymentResultModel
    {
        public string Id { get; set; }

        public bool IsAlreadyBeenPaid { get; set; }
    }
}
