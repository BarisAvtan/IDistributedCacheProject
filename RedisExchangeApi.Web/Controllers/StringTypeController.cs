using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;

        private IDatabase db;

        public StringTypeController(RedisService redisService)
        {
         
            _redisService = redisService;
            //db = _redisService.GetDb(0);

        }

        public IActionResult Index()
        {
            _redisService.Connect();
            db = _redisService.GetDb(0);
            db.StringSet("name", "Baris BD");
            db.StringSet("name2", 100);
            return View();
        }

        public IActionResult Show()
        {
            _redisService.Connect();
           
            db = _redisService.GetDb(0);

            var name = db.StringGet("name");
            
            var name1 = db.StringGetRange("name",0,3);

            var name2 = db.StringLength("name");

            db.StringIncrement("name2",5);

            db.StringDecrementAsync("name2", 5);

           var count = db.StringDecrementAsync("name2", 5).Result;//sonucu değişkene al

            db.StringDecrementAsync("name2", 5).Wait();//sonucla ilgilenmiyorum asenkron biçimde çalıştır.

            if (name.HasValue)
            {
                ViewBag.name = name.ToString();
            }
            return View();
        }
    }
}
