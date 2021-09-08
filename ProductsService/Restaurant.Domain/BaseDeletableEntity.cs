
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain
{
    public abstract class BaseDeletableEntity<TKey>
    {
        public TKey Id { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime? DisabledOn { get; set; }
    }
}
