using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisExampleApp.API.Models;
using RedisExampleApp.API.Repository;
using RedisExampleApp.API.Services;
using RedisExampleApp.Cache;
using StackExchange.Redis;

namespace RedisExampleApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Product2Controller : ControllerBase
    {
        private readonly IProductService _productService;

        public Product2Controller(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productService.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            return Created(string.Empty, await _productService.CreateAsync(product));
        }
    }
}
