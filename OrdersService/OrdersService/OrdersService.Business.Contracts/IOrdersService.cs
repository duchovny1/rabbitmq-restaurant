using OrdersService.Domain.ViewModels.Request;
using OrdersService.Domain.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Business.Contracts
{
    public interface IOrdersService
    {
        bool IsOrderExists(long orderNumber);

        Task CreateAnOrder(OrdersInputModelRequest order);

        Task ChangeOrderStatus(ChangeOrderStatusRequest request);

        decimal CalculateAmountOfOrder(long orderNumber);
    }
}
