using StackExchange.Redis.Extensions.Core;

namespace BaseBallVR.Redis.DB
{
    public interface IBase
    {
        ICacheClient GetCacheClient();
    }
}