using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class BaseController : Controller
    {

        protected readonly RedisService _redisService;

        protected IDatabase db;

        public BaseController(RedisService redisService)
        {

            _redisService = redisService;
        }
     
    }
}
