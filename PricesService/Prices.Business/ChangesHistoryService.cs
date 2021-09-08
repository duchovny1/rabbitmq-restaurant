using EventBus;
using Microsoft.EntityFrameworkCore;
using Prices.Data.Contracts;
using Prices.Domain.Models;
using Prices.Domain.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Prices.Business
{
    public class ChangesHistoryService : IChangesHistoryService
    {
        private readonly IRepository<ChangesHistory> _repository;
        private readonly IRepository<Price> _priceRepository;
        private readonly IEventBus _bus;
        private const string message = "The price for {0} with id {1} is setted to {2}lv.";
        private const string messageForChangedPrice = "The price for {0} with id {1} was changed from {2} to new price of {3}lv.";

        private const string topic_name = "log.info";

        public ChangesHistoryService(
            IRepository<ChangesHistory> repository,
            IRepository<Price> priceRepository,
            IEventBus bus)
        {
            _repository = repository;
            _priceRepository = priceRepository;
            _bus = bus;
        }
        public async Task AddToHistory(long productId, SetProductPriceRequest request)
        {
            var isNewProduct = await _priceRepository.All().FirstOrDefaultAsync(x => x.ProductId == productId);

            if (isNewProduct is null)
            {
                var newProductPrice = new ChangesHistory()
                {
                    ProductId = productId,
                    ProductName = request.ProductName,
                    NewPrice = request.Price
                };

                await _repository.AddAsync(newProductPrice);
                await _repository.SaveChangesAsync();

                _bus.Publish(string.Format(message, newProductPrice.ProductName, newProductPrice.ProductId, newProductPrice.NewPrice)
                    , topic_name);
            }
            else
            {
                var newProductPrice = new ChangesHistory()
                {
                    ProductId = productId,
                    ProductName = request.ProductName,
                    PreviousPrice = isNewProduct.PriceAmount,
                    NewPrice = request.Price,
                    ModifiedOn = DateTime.UtcNow
                };

                  await _repository.AddAsync(newProductPrice);
                  await _repository.SaveChangesAsync();

                 _bus.Publish(string.Format(messageForChangedPrice, newProductPrice.ProductName,
                             newProductPrice.ProductId,
                             newProductPrice.PreviousPrice,
                             newProductPrice.NewPrice),
                             topic_name);
            }
        }
    }
}
