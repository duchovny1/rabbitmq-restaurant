using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Domain.ViewModels.Response
{
    public class IsOrderExistModel
    {
        public IsOrderExistModel(bool isOrderExist)
        {
            IsOrderExists = isOrderExist;
        }
        public bool IsOrderExists { get; set; }
    }
}
