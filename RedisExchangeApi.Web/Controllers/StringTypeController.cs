using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;

namespace RedisExchangeApi.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        public StringTypeController(RedisService redisService)
        {
            _redisService= redisService;
        }
        public IActionResult Index()
        {
            _redisService.Connect();
            var db = _redisService.GetDb(0);
            db.StringSet("name", "Baris BD");
            db.StringSet("name2", 100);
            return View();
        }
    }
}
