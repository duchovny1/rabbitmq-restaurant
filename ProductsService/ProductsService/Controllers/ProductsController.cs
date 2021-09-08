using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Business.Contracts.Services;
using Restaurant.ViewModels.Request;

namespace ProductsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productsService;
        private readonly IEventBus _eventBus;

        public ProductsController(IProductService productService, IEventBus eventBus)
        {
            _productsService = productService;
            _eventBus = eventBus;
        }

        public IActionResult Index()
        {
            _eventBus.Publish("Message was sent and recieved", "log.info");
            return Ok("its working");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllAsync(long id)
        {
            var menuItems = await this._productsService.GetAllByCategoryId(id);

            return this.Ok(menuItems);
        }
        
        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            var categories = this._productsService.GetCategories();
            _eventBus.Publish("Categories shown.", "log.info");
            return this.Ok(categories);
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(MenuItemInfoRequest menuItem)
        {
            await _productsService.AddAsync(menuItem);

            return Ok();
        }

        [HttpGet("/exists/{productId}")]
        public async Task<IActionResult> IsProductExists(long productId)
        {
            var result = await _productsService.IsMenuItemExistsAsync(productId);

            if(result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}