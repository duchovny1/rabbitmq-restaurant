using Prices.Domain.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Prices.Business.Contracts
{
    public interface IPricesService
    {
        Task GetProductPrice(long productId);

        Task<bool> SetPriceToAProduct(long productId, SetProductPriceRequest request);

        Task<bool> UpdatePriceToAProduct(long productId, SetProductPriceRequest request);

        Task<bool> IsProductPriceSet(long productId);
    }
}
