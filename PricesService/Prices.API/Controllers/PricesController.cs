using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Prices.Business;
using Prices.Domain.ViewModels.Request;
using Prices.Domain.ViewModels.Response;

namespace Prices.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PricesController : ControllerBase
    {
        private readonly IPricesService _priceService;

        public PricesController(IPricesService _priceService)
        {
            this._priceService = _priceService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("ok");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPriceForProduct(long productId)
        {
            var result = await this._priceService.GetProductPriceAsync(productId);

            if(result == null)
            {
                return NotFound($"The price for a product with id {productId} is not setted");
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SetPriceToAProduct(long productId, [FromBody] SetProductPriceRequest request)
        {
            var result = await _priceService.SetPriceToAProduct(productId, request);

            if (result)
            {
                return Ok();
            }

            return BadRequest("Something went wrong. The product price cannot be change");
       }
    }
}