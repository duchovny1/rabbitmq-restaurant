using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Prices.Domain.Models
{
    public class ChangesHistory : IAuditInfo
    {
        public long Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal PreviousPrice { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal NewPrice { get; set; }
        public string ProductName { get; set; }
        public long ProductId { get; set; }
    }
}
