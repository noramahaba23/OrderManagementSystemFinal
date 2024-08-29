using StackExchange.Redis;
using NRedisStack;
using NRedisStack.RedisStackCommands;




namespace API.API
{


    public class RedisService
    {
        private readonly ConnectionMultiplexer _redis;

        public RedisService(IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetSection("Redis")["ConnectionString"];
            _redis = ConnectionMultiplexer.Connect(redisConnectionString);
        }







        public void SetValue(string key, string value)
        {
            var db = _redis.GetDatabase();
            db.StringSet(key, value);
        }

        public string GetValue(string key)
        {
            var db = _redis.GetDatabase();
            return db.StringGet(key);
        }
    }
}

