using Prices.Domain.ViewModels.Request;
using Prices.Domain.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Prices.Business
{
    public interface IPricesService
    {
        Task<ProductPriceResponse> GetProductPriceAsync(long productId);

        Task<bool> SetPriceToAProduct(long productId, SetProductPriceRequest request);

        Task<bool> UpdatePriceToAProduct(long productId, SetProductPriceRequest request);

        Task<bool> IsProductPriceSet(long productId);
    }
}
