using Newtonsoft.Json;
using OrdersService.Business.Contracts;
using OrdersService.Data.Contracts;
using OrdersService.Domain.Enums;
using OrdersService.Domain.Models;
using OrdersService.Domain.ViewModels.Request;
using OrdersService.Domain.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Business
{
    public class OrdersService : IOrdersService
    {
        private readonly IRepository<Order> _ordersRepository;
        private readonly IRepository<OrderProduct> _orderProductRepository;
        private readonly HttpClient _httpClient;


        public OrdersService(IRepository<Order> ordersRepository,
            IRepository<OrderProduct> orderProductRepository,
            HttpClient httpClient)
        {
            _ordersRepository = ordersRepository;
            _orderProductRepository = orderProductRepository;
            _httpClient = httpClient;
        }

        public async Task ChangeOrderStatus(ChangeOrderStatusRequest request)
        {
            var status = Enum.TryParse(request.OrderStatus, out OrderStatus orderStatus);

            if(!status)
            {
                throw new ArgumentException($"Cannot change the order status for order {request.OrderNumber}");
            }

            var isOrderExists = IsOrderExists(request.OrderNumber);

            if (!isOrderExists)
            {
                throw new ArgumentException($"The order {request.OrderNumber} does not exist in the database");
            }

            var order = _ordersRepository.All().FirstOrDefault(x => x.OrderNumber == request.OrderNumber);

            order.OrderStatus = orderStatus;

            _ordersRepository.Update(order);

            await _ordersRepository.SaveChangesAsync();
        }

        public async Task CreateAnOrder(OrdersInputModelRequest order)
        {
            ValidateOrder(order);

            var orderProducts = new List<OrderProduct>();

            var orderToAdd = new Order()
            {
                IsPaid = order.IsPaid,
                OrderStatus = OrderStatus.OrderRecieved
            };

            await _ordersRepository.AddAsync(orderToAdd);
            await _ordersRepository.SaveChangesAsync();

            foreach (var product in order.Products)
            {
                var result = await ValidateProduct(product);

                if (result)
                {
                    var productInOrder = new OrderProduct()
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        Quantity = product.Quantity,
                        Order = orderToAdd
                    };

                    await _orderProductRepository.AddAsync(productInOrder);
                    await _orderProductRepository.SaveChangesAsync();

                    orderProducts.Add(productInOrder);
                }
            }

            orderToAdd.Products = orderProducts;

            _ordersRepository.Update(orderToAdd);
            await _ordersRepository.SaveChangesAsync();
        }

        private async Task<bool> ValidateProduct(ProductModelRequest product)
        {
            if (product.ProductId == 0)
            {
                throw new ArgumentException("The product id is not correct");
            }

            if (string.IsNullOrEmpty(product.ProductName))
            {
                throw new ArgumentException("There is not given product name");
            }

            if (product.Quantity == 0)
            {
                throw new ArgumentException("Cannot make an order with 0 quantity");
            }

            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:5001/exists/{product.ProductId}");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                        //var result = await response.Content.ReadAsStringAsync();

                        //var productModel = JsonConvert.DeserializeObject<ProductModel>(result);

                        //if (product.ProductName != productModel.ProductName)
                        //{
                        //    throw new ArgumentException($"The given product name for product with {product.ProductId} is not correct");
                        //}

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (HttpRequestException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

        }

        private void ValidateOrder(OrdersInputModelRequest order)
        {
            if (string.IsNullOrEmpty(order.OrderStatus))
            {
                throw new ArgumentNullException("Order status is missing.");
            }

            if (order.Products is null && order.Products.Count() <= 0)
            {
                throw new ArgumentException("You cannot make an order without products.");
            }
        }

        public bool IsOrderExists(long orderNumber)
        {
            return _ordersRepository.All().Any(x => x.OrderNumber == orderNumber);
        }

        public decimal CalculateAmountOfOrder(long orderNumber)
        {

            return 16;
            //var order =_ordersRepository.All().FirstOrDefault(x => x.OrderNumber == orderNumber);

            //if(order is null)
            //{
            //    throw new ArgumentNullException($"The order with number {orderNumber} does not exist");
            //}

            //decimal totalPrice = 0;

            //foreach (var product in order.Products)
            //{
            //    var response = await _httpClient.GetAsync($"https://localhost.com:5003/api/prices/{product.ProductId}");

            //    var responseBody = await response.Content.ReadAsStringAsync();
            //    var productPrice = JsonConvert.DeserializeObject<PriceToProductModel>(responseBody);

            //    decimal currentPrice = product.Quantity * productPrice.Price;
            //    totalPrice += currentPrice;
            //}

            //var result = new TotalAmountResponse(orderNumber, totalPrice);

            //return result;
        }
    }
}
