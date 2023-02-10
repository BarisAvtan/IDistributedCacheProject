
using RedisExampleApp.API.Models;
using RedisExampleApp.API.Repository;

namespace RedisExampleApp.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productrepository)
        {
            _productRepository = productrepository;
        }

        public async Task<Product> CreateAsync(Product product)
        {
          return await _productRepository.CreateAsync(product);   
        }

        public async Task<List<Product>> GetAsync()
        {
            return await _productRepository.GetAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }
    }
}
