using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.ViewModels.Response
{
    class MenuItemsPerCatagoryResponse
    {
        public long CategoryId { get; set; }

        public MenuItemInfoResponse[] MenuItems { get; set; }
    }
}
