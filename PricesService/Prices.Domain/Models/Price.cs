using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Prices.Domain.Models
{
    public class Price : BaseEntity<long>
    {
        public string ProductName { get; set; }
        public long ProductId { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal PriceAmount { get; set; }

    }
}
