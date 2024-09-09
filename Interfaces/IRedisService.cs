using StackExchange.Redis;

namespace CommonLibrary.Interfaces
{
    public interface IRedisService
    {
        IDatabase redisDb { get; }

        /// <summary> Redis上鎖 </summary>
        Task<bool> AcquireLockAsync();

        /// <summary> Redis釋放鎖 </summary>
        Task ReleaseLockAsync();
    }
}
