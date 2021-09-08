using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentService.Domain.Enums
{
    public enum PaymentResult
    {
         Success = 1,
         Failed = 2,
         AlreadyBeenPaid = 3
    }
}
