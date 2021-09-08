using PaymentService.Domain;
using PaymentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Business.Contracts
{
    public interface IPaymentService
    {
       Task<bool> IsOrderAlreadyBeenPaid(long orderNumber);

       Task<PaymentResultModel> ProccessAPayment(CreateAPaymentRequest request);

       Task<PaymentDetailsResponse> GetPaymentDetails(string paymentId);
    }
}
