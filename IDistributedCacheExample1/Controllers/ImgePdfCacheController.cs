using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistributedCacheExample1.Controllers
{
    public class ImgePdfCacheController : Controller
    {
        private IDistributedCache _distributedCache;

        public ImgePdfCacheController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public IActionResult ImageCache()
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/car1.jpeg");
            
            byte[] imageByte = System.IO.File.ReadAllBytes(imagePath);
            
            _distributedCache.Set("resim1", imageByte);
            
            return View();
        }

        public IActionResult Index()
        {
            //ImageUrl'i bu sayfada gösterelim
            return View();


        }

        public IActionResult ImageUrl()
        {

            byte[] imageByte = _distributedCache.Get("resim1");

            return File(imageByte, "imege/jpeg");
        }
    }
}
