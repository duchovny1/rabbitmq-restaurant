using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.ViewModels.Request
{
    public class MenuItemInfoRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IsEnabled { get; set; }
        public long? CategoryId { get; set; }
    }
}
