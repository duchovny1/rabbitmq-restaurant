using EventBus;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Prices.Data.Contracts;
using Prices.Domain.Models;
using Prices.Domain.ViewModels.Request;
using Prices.Domain.ViewModels.Response;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Prices.Business
{
    public class PriceService : IPricesService
    {
        private readonly IRepository<Price> _priceRepository;
        private readonly IEventBus _bus;

        public PriceService(IRepository<Price> priceRepository, IEventBus _bus)
        {
            _priceRepository = priceRepository;
            this._bus = _bus;
        }
        public async Task<ProductPriceResponse> GetProductPriceAsync(long productId)
         =>  await _priceRepository.All().Where(x => x.ProductId == productId)
                .Select(x => new ProductPriceResponse
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    PriceAmount = x.PriceAmount
                })
                .FirstOrDefaultAsync();

        public async Task<bool> IsProductPriceSet(long productId)
           => await _priceRepository.All().AnyAsync(x => x.ProductId == productId && x.PriceAmount != 0.0m);
        

        public async Task<bool> SetPriceToAProduct(long productId, SetProductPriceRequest request)
        {
            var isPriceAlreadySetted = await _priceRepository.All().AnyAsync(x => x.ProductId == productId && x.PriceAmount != 0.0m);

            if(!isPriceAlreadySetted)
            {
                Price price = new Price()
                {
                    ProductId = productId,
                    ProductName = request.ProductName,
                    PriceAmount = request.Price
                };

                _priceRepository.Add(price);
                _priceRepository.SaveChanges();

                var priceSerialized = JsonConvert.SerializeObject(price);

                _bus.Publish(priceSerialized, "changes.history.log");
                // with this publish we create ChangeHistory record 

                return true;
            }
            else
            {
                try
                {
                    await UpdatePriceToAProduct(productId, request);
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }

        public async Task<bool> UpdatePriceToAProduct(long productId, SetProductPriceRequest request)
        {
            // take it from the database // change the price // publish to a bus so we can add a new record in changes history table

            var product = await _priceRepository.All().FirstOrDefaultAsync(x => x.ProductId == productId);
            var oldPrice = product.PriceAmount;

            product.PriceAmount = request.Price;

            _priceRepository.Update(product);
            
            await _priceRepository.SaveChangesAsync();

            var priceSerialized = JsonConvert.SerializeObject(product);

            _bus.Publish(priceSerialized, "changes.history.log");

            return true;
        }
    }
}
