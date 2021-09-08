using System;
using System.Collections.Generic;
using System.Text;

namespace Prices.Domain
{
    public abstract class BaseEntity<TKey> : IAuditInfo
    {
        public TKey Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set ; }
    }
}
