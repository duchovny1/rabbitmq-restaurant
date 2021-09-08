using EventBus;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PaymentService.Business.Contracts;
using PaymentService.Data;
using PaymentService.Domain;
using PaymentService.Domain.Enums;
using PaymentService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Business
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IUsersService _usersService;
        private readonly HttpClient _httpClient;
        private readonly IEventBus _bus;

        private const string message = "The order {0} for {1} - {2} has been paid";

        public PaymentService(IRepository<Payment> paymentRepository, 
            IUsersService usersService, 
            HttpClient httpClient, 
            IEventBus bus)
        {
            _paymentRepository = paymentRepository;
            _usersService = usersService;
            _httpClient = httpClient;
            _bus = bus;
        }

        public async Task<PaymentDetailsResponse> GetPaymentDetails(string paymentId)
        {
            var payment = await _paymentRepository.All().FirstOrDefaultAsync(x => x.Id == paymentId && x.IsBeenProccessed);

            if(payment == null)
            {
                throw new ArgumentNullException("This payment does not exist");
            }

            var result = new PaymentDetailsResponse()
            {
                Id = payment.Id,
                OrderNumber = payment.OrderNumber,
                TotalPrice = payment.Price,
                UserDetails = new UserModelRequest()
                {
                    FirstName = payment.User.FirstName,
                    LastName = payment.User.LastName,
                    AddressLine1 = payment.User.AddressLine1,
                    AddressLine2 = payment.User.AddressLine2,
                    TelephoneNumber = payment.User.TelephoneNumber
                }
            };

            return result;
        }

        public async Task<bool> IsOrderAlreadyBeenPaid(long orderNumber)
        {
            return await _paymentRepository.All().AnyAsync(x => x.OrderNumber == orderNumber && x.IsBeenProccessed);
        }

        public async Task<PaymentResultModel> ProccessAPayment(CreateAPaymentRequest request)
        {
            //ValidateRequest(request);

            //var result = new PaymentResultModel();
            //result.Id = string.Empty;

            //var isAlreadyBeenPaid = await IsOrderAlreadyBeenPaid(request.OrderNumber);

            //if(isAlreadyBeenPaid)
            //{
            //     result.IsAlreadyBeenPaid = true;
            //     return result;
            //}

            //var user = await _usersService.GetUserAsync(request.UserDetails);

            //if(user == null)
            //{
            //    try
            //    {
            //       user = await _usersService.CreateUserAsync(request.UserDetails);
            //    }
            //    catch(DBConcurrencyException)
            //    {
            //        return result;
            //    }
            //}

            //Enum.TryParse(request.PaymentMethod, out PaymentMethod paymentMethod);

            //decimal totalAmountForOrder = await GetTotalAmountForOrder(request.OrderNumber);

            //var payment = new Payment()
            //{
            //    OrderNumber = request.OrderNumber,
            //    PaymentMethod = paymentMethod,
            //    User = user,
            //    IsBeenProccessed = true,
            //    Price = totalAmountForOrder
            //};

            //try
            //{
            //   await _paymentRepository.AddAsync(payment);
            //   await _paymentRepository.SaveChangesAsync();

            //    result.Id = payment.Id;
            //}
            //catch (Exception)
            //{
            //    // may be the exception should be logged
            //}

            _bus.PublishMessage(string.Format(message, request.OrderNumber, request.UserDetails.FirstName, request.UserDetails.LastName), "log_info");
            _bus.PublishMessage(string.Format(message, request.OrderNumber, request.UserDetails.FirstName, request.UserDetails.LastName), "log_info");

            return new PaymentResultModel();
        }

        private async Task<decimal> GetTotalAmountForOrder(long orderNumber)
        {
           //var response = await _httpClient.GetAsync("https://localhost.com:5003/api/orders/totalAmount/{orderNumber}");

           // response.EnsureSuccessStatusCode();

           // var responseBody = await response.Content.ReadAsStringAsync();

           // var orderPrice = JsonConvert.DeserializeObject<TotalAmountResponse>(responseBody);

            return 16;
        }

        private void ValidateRequest(CreateAPaymentRequest request)
        {
            if(request == null)
            {
                throw new ArgumentNullException();
            }

            if(request.UserDetails == null)
            {
                throw new ArgumentNullException("The user details are missing.");
            }

            if(request.OrderNumber == 0)
            {
                throw new ArgumentException("There is no order linked to the payment!");
            }

            if (string.IsNullOrEmpty(request.PaymentMethod))
            {
                throw new ArgumentException("Payment should contain payment method!");
            }

            if(!Enum.TryParse(request.PaymentMethod, out PaymentMethod _))
            {
                throw new ArgumentException("The payment method is not valid.");
            }
        }
    }
}
