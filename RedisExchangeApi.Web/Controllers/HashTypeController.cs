using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;

namespace RedisExchangeApi.Web.Controllers
{
    public class HashTypeController : BaseController
    {
        public string hashKey { get; set; } = "sozluk";
        public HashTypeController(RedisService redisService) : base(redisService)
        {
        }

        public IActionResult Index()
        {
            _redisService.Connect();
            db = _redisService.GetDb(1);
            Dictionary <string,string> list = new Dictionary<string,string>();
            if (db.KeyExists(hashKey))
            {
                db.HashGetAll(hashKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name, x.Value);
                });
            }
            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string name,string value)
        {
            _redisService.Connect();
            db = _redisService.GetDb(1);
            db.HashSet(hashKey, name, value);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            _redisService.Connect();
            db = _redisService.GetDb(1);
            db.HashDelete(hashKey, name);
            return RedirectToAction("Index");
        }
    }
}
