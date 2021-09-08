using EventBus;
using Microsoft.EntityFrameworkCore;
using PaymentService.Business.Contracts;
using PaymentService.Data;
using PaymentService.Domain;
using PaymentService.Domain.Enums;
using PaymentService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Business
{
    public class UsersService : IUsersService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IEventBus _bus;

        public UsersService(IRepository<User> userRepository, IEventBus bus)
        {
            _userRepository = userRepository;
            _bus = bus;
        }
        public async Task<User> CreateUserAsync(UserModelRequest model)
        {
            // in payment service - add paymethod we called the GetUserAsync method
            // we use this method only for creating the user
            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                TelephoneNumber = model.TelephoneNumber
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            _bus.PublishMessage($"User with {user.FirstName} - {user.LastName} has been registered.", "log_info");


            return user;
        }

        public async Task<User> GetUserAsync(UserModelRequest model)
            =>  await _userRepository.All().FirstOrDefaultAsync(
                x => x.FirstName == model.FirstName
                && x.LastName == model.LastName
                && x.TelephoneNumber == model.TelephoneNumber);
        
    }
}
