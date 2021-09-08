using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentService.Domain.Models
{
    public class User
    {

        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Payments = new HashSet<Payment>();
        }

        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string TelephoneNumber { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

    }
}
