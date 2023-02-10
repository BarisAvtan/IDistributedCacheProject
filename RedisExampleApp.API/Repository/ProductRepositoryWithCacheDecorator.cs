using RedisExampleApp.API.Models;
using RedisExampleApp.Cache;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExampleApp.API.Repository
{
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {
        //DECAROTOR DESIGN PATTERN
        private const string productKey = "productCaches";
        private readonly IProductRepository _productRepository;
        private readonly IDatabase _cacheRepository;
        private readonly RedisService _redisService;

        public ProductRepositoryWithCacheDecorator(IProductRepository productRepository, RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _cacheRepository = redisService.GetDb(1);
        }
        public async Task<Product> CreateAsync(Product product)
        {
            var newProduct = await _productRepository.CreateAsync(product);

            if (await _cacheRepository.KeyExistsAsync(productKey))//eger data cache'de varsa bu datayı cache 'de ekle
            {
                await _cacheRepository.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(product));
            }

            return newProduct;
        }

        public async Task<List<Product>> GetAsync()
        {
            if (!await _cacheRepository.KeyExistsAsync(productKey))
            {
                return await LoadToCacheFromDbAsync();
            }
            var products = new List<Product>();

            var cacheInRedisProducs = (await _cacheRepository.HashGetAllAsync(productKey)).ToList();

            foreach (var productItem in cacheInRedisProducs)
            {
                var product = JsonSerializer.Deserialize<Product>(productItem.Value);

                products.Add(product);
            }
            return products;
        }

        public  async Task<Product> GetByIdAsync(int id)
        {
            if (_cacheRepository.KeyExists(productKey))
            {
                var product = await _cacheRepository.HashGetAsync(productKey,id);

                return product.HasValue ? JsonSerializer.Deserialize<Product>(product): null;
            }
            var products = await LoadToCacheFromDbAsync();

            return products.FirstOrDefault(x => x.Id == id);
        }

        private async Task<List<Product>> LoadToCacheFromDbAsync()//db den cache'e yükle
        {
            var products = await _productRepository.GetAsync();

            products.ForEach(product =>
            {
                _cacheRepository.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(product));
            });

                return products;
        }



    }
}
