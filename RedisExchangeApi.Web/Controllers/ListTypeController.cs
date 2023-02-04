using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;

        private IDatabase db;
        private string listKey = "names";
        public ListTypeController(RedisService redisService)
        {

            _redisService = redisService;
        }

        public IActionResult Index()
        {
            _redisService.Connect();
            db = _redisService.GetDb(0);
            List<string> nameList = new List<string>();
            if (db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList().ForEach(x =>
                {
                    nameList.Add(x.ToString());
                });
            }
            return View(nameList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            _redisService.Connect();
            db = _redisService.GetDb(0);
            db.ListRightPush(listKey, name);

            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            _redisService.Connect();
            db = _redisService.GetDb(0);
            db.ListRemoveAsync(listKey, name).Wait();

            return RedirectToAction("Index");
        }

        public IActionResult DeleteFirstItem(string name)
        {
            _redisService.Connect();
            db = _redisService.GetDb(0);
            db.ListLeftPopAsync(listKey).Wait();

            return RedirectToAction("Index");
        }
    }
}
