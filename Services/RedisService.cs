using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace CommonLibrary.Services
{
    public class RedisService
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<RedisService> _log;
        private readonly ConnectionMultiplexer _connection;
        private readonly Lazy<IDatabase> _redisDb;
        private readonly string _lockKey = "queueLockKey";
        private readonly string redisKey;
        private readonly TimeSpan _expiry = new TimeSpan(0, 1, 00);//設定失效時間為1分鐘

        public IDatabase redisDb => _redisDb.Value;

        public RedisService(IConfiguration configuration, ILogger<RedisService> log)
        {
            redisKey = $"queueList_{DateTime.Now.ToString("MMdd")}";
            _configuration = configuration;
            _connection = ConnectionMultiplexer.Connect(_configuration.GetConnectionString("Redis") ?? string.Empty);
            _redisDb = new Lazy<IDatabase>(() => _connection.GetDatabase());
            _log = log;
        }



        /// <summary>
        /// Redis上鎖
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AcquireLockAsync()
        {
            try
            {
                return await redisDb.StringSetAsync(_lockKey, redisKey, _expiry, When.NotExists);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Redis釋放鎖
        /// </summary>
        /// <returns></returns>
        public async Task ReleaseLockAsync()
        {
            try
            {
                await redisDb.KeyDeleteAsync(_lockKey);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw;
            }
        }

    }

}
