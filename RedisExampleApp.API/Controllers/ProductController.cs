using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisExampleApp.API.Models;
using RedisExampleApp.API.Repository;
using RedisExampleApp.Cache;
using StackExchange.Redis;

namespace RedisExampleApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        private readonly RedisService _redisService;

        private readonly IDatabase _database;

        public ProductController(IProductRepository productRepository, RedisService redisService,IDatabase database)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            var db = _redisService.GetDb(0);
            db.StringSet("isim", "baris");

            //2.yol
           _database= database;
            database.StringSet("soyad", "yildiz");


        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productRepository.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productRepository.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult>Create(Product product)
        {
            return Created(string.Empty,await _productRepository.CreateAsync(product));
        } 
    }
}
