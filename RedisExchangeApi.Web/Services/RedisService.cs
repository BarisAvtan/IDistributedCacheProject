using StackExchange.Redis;

namespace RedisExchangeApi.Web.Services
{
    public class RedisService
    {
        private readonly string _redisHost;
        private readonly string _redisPort;
        public IDatabase db { get; set; }

        private ConnectionMultiplexer _redis;//ConnectionMultiplexer clası üzerinden redis ile haberleşilir.

        public RedisService(IConfiguration configuration)
        
        {
            _redisHost = configuration["Redis:Host"];

            _redisPort = configuration["Redis:Port"];
            
            Connect();
           
        }

        public void Connect()//uygulama ayağa kalktıgında bu connect metodu çagrılmalı yani program.cs 'e eklenmeli
        {
            var configString = $"{_redisHost}:{_redisPort}";

            _redis = ConnectionMultiplexer.Connect(configString);

        }

        public IDatabase GetDb(int dbNumber) //redis serverda 1 den 15 'e kadar olan dblerden birini şeçebilmek için yazıldı.
        {
            return _redis.GetDatabase(dbNumber);
        }
    }
}
