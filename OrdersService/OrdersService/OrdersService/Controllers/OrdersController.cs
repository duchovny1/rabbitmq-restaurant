using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrdersService.Business.Contracts;
using OrdersService.Domain.Models;
using OrdersService.Domain.ViewModels.Request;
using OrdersService.Domain.ViewModels.Response;

namespace OrdersService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost("/create")]
        public async Task<IActionResult> CreateAnOrder([FromBody] OrdersInputModelRequest request)
        {
            if(request == null)
            {
                throw new ArgumentNullException("You cannot make an empty order.");
            }

            try
            {
               await _ordersService.CreateAnOrder(request);

               return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        
        [HttpPut("/orderstatus")]
        public async Task<IActionResult> ChangeOrderStatus([FromBody] ChangeOrderStatusRequest request)
        {
            try
            {
                await _ordersService.ChangeOrderStatus(request);

                return Ok($"The status for order {request}");
            }
            catch(ArgumentException)
            {
                return BadRequest("Invalid arguments were sent");
            }
            catch(Exception)
            {
                return BadRequest("Something went wrong. Cannot change the order status");
            }
        }

        [HttpGet("/exists/{orderNumber}")]
        public IActionResult IsOrderExists(long orderNumber)
        {
            var result = _ordersService.IsOrderExists(orderNumber);

            return Ok(new IsOrderExistModel(result));
        }

        [HttpGet("/totalAmount/{orderNumber}")]
        public async Task<IActionResult> GetOrderTotalAmount(long orderNumber)
        {
            var result = _ordersService.CalculateAmountOfOrder(orderNumber);

            return Ok(result);
        }
    }
}