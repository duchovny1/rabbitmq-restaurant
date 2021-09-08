using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.API.Consumers;
using PaymentService.API.Producers;
using PaymentService.Business.Contracts;
using PaymentService.Domain;
using PaymentService.Domain.Enums;

namespace PaymentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IEventBus _bus;

        public PaymentsController(IPaymentService paymentService, IEventBus bus)
        {
            _paymentService = paymentService;
            _bus = bus;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAPayment([FromBody] CreateAPaymentRequest request)
        {
            _bus.PublishMessage(request.OrderNumber.ToString(), "process_payment");

            return Ok($"Transaction was completed.");
        }

        [HttpPost("/process")]
        public async Task<IActionResult> ProcessPayment([FromBody] CreateAPaymentRequest request)
        {

            await _paymentService.ProccessAPayment(request);
            _bus.PublishMessage(request.OrderNumber.ToString(), "process_payment");

            return Ok($"Transaction was completed.");
        }


        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPaymentDetails(string paymentId)
        {
            var result = await _paymentService.GetPaymentDetails(paymentId);

            return Ok(result);
        }
       




        [HttpGet("/exists/{orderNumber}")]
        public async Task<IsBeenPaidResultObject> IsAlreadyBeenPaid(long orderNumber)
        {
            var result = await _paymentService.IsOrderAlreadyBeenPaid(orderNumber);

            return new IsBeenPaidResultObject() { IsAlreadyBeenPaid = result };
        }
    }
}