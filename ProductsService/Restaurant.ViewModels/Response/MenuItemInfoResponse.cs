using Restaurant.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.ViewModels.Response
{
    public class MenuItemInfoResponse
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public string PictureURL { get; set; }
    }
}
