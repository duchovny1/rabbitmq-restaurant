using PaymentService.Domain;
using PaymentService.Domain.Enums;
using PaymentService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Business.Contracts
{
    public interface IUsersService
    {
        Task<User> GetUserAsync(UserModelRequest model);

        Task<User> CreateUserAsync(UserModelRequest model);
    }
}
