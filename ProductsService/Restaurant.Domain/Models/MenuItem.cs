using Restaurant.Domain.Enums;
using System.Collections.Generic;

namespace Restaurant.Domain.Models
{
    public class MenuItem : BaseDeletableEntity<long>
    {
        public MenuItemTypes MenuItemType { get; set; }

        public string Description { get; set; }

        public long CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public string Name { get; set; }

        public string PictureURL { get; set; }
    }
}
