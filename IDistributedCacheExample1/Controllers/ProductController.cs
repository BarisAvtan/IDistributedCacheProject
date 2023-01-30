using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistributedCacheExample1.Controllers
{
    public class ProductController : Controller
    {
        private IDistributedCache _distribbutedCache;

        public ProductController(IDistributedCache distribbutedCache)
        {
            _distribbutedCache = distribbutedCache;
        }
        public IActionResult Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            _distribbutedCache.SetString("name", "Baris", cacheEntryOptions);//bu keye sahip olan değer memory'de 1 dk duracak.
            return View();
        }


        public async Task<IActionResult> Index2()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            await _distribbutedCache.SetStringAsync("surname", "Dogan", cacheEntryOptions);//bu keye sahip olan değer memory'de 1 dk duracak.
            return View();
        }
        public IActionResult Show()
        {
            string name = _distribbutedCache.GetString("name");
            ViewBag.name = name;
            return View();

        }

        public IActionResult RemoveData()
        {
            _distribbutedCache.Remove("name");
            return View();

        }
    }
}
