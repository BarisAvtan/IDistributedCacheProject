using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
namespace IDistributedCacheExample.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _cache;
        public ProductsController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
