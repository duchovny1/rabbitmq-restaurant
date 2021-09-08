using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Models
{
    public class Category : BaseDeletableEntity<long>
    {
        public Category()
        {
            this.MenuItems = new HashSet<MenuItem>();
        }

        public string Name { get; set; }    

        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}
