using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentService.Domain
{
    public class UserModelRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string TelephoneNumber { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }
    }
}
