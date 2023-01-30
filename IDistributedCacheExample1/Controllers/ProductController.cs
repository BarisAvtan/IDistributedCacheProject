using IDistributedCacheExample1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace IDistributedCacheExample1.Controllers
{
    public class ProductController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductController(IDistributedCache distribbutedCache)
        {
            _distributedCache = distribbutedCache;
        }
        public IActionResult Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            _distributedCache.SetString("name", "Baris", cacheEntryOptions);//bu keye sahip olan değer memory'de 1 dk duracak.
            return View();
        }


        public async Task<IActionResult> IndexOne()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            await _distributedCache.SetStringAsync("surname", "Dogan", cacheEntryOptions);//bu keye sahip olan değer memory'de 1 dk duracak.
            return View();
        }

        public async Task<IActionResult> IndexTwo()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            Product product = new Product { Id = 1 ,Name = "Kalem", Price=100};

            string jsonProduct = JsonConvert.SerializeObject(product);

            await _distributedCache.SetStringAsync("product:1" , jsonProduct, cacheEntryOptions);

            return View();
        }

        public IActionResult ShowIndexTwo()
        {
            string jsonProduct = _distributedCache.GetString("product:1");
            Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);
            ViewBag.product = p;
            return View();

        }

        public async Task<IActionResult> ByteExample()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            Product product = new Product { Id = 1, Name = "Kalem", Price = 100 };
           
            string jsonProduct = JsonConvert.SerializeObject(product);
            
            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

            _distributedCache.Set("product:1", byteProduct);

            return View();
        }


        public async Task<IActionResult> ByteConvertString()
        {


            Byte[] byteProduct = _distributedCache.Get("product:1");

            string jsonProduct = Encoding.UTF8.GetString(byteProduct);

            Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);

            ViewBag.product = p;

            return View();
        }
        public IActionResult Show()
        {
            string name = _distributedCache.GetString("name");
            ViewBag.name = name;
            return View();

        }

        public IActionResult RemoveData()
        {
            _distributedCache.Remove("name");
            return View();

        }

    }
}
