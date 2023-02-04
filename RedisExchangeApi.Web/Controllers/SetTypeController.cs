using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;

        private IDatabase db;
        private string listKey = "hashnames";
        public SetTypeController(RedisService redisService)
        {

            _redisService = redisService;
        }
        public IActionResult Index()
        {
            _redisService.Connect();
            db = _redisService.GetDb(1);

            HashSet<string> nameList = new HashSet<string>();

            if (db.KeyExists(listKey))
            {
                db.SetMembers(listKey).ToList().ForEach(x =>
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
            db = _redisService.GetDb(1);

            if (!db.KeyExists(listKey))
            {
                db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));
            }
           
            db.SetAdd(listKey, name);
            return RedirectToAction("Index");
        }
        public  async Task< IActionResult> DeleteItem(string name)
        {
            _redisService.Connect();
            db = _redisService.GetDb(1);
            db.SetRemoveAsync(listKey, name);
            return RedirectToAction("Index");

        }
    }
}
