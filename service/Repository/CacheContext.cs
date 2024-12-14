namespace service;

using StackExchange.Redis;

public class CacheContext
{
    private readonly ConnectionMultiplexer _redis;
    private IDatabase _database { get; }
    public CacheContext(string connectionString)
    {
        _redis = ConnectionMultiplexer.Connect(connectionString);
        _database = _redis.GetDatabase();
    }
    public void Close()
    {
        _redis.Close();
    }

    public void Set(string key, RedisValue value)
    {
        _database.StringSetAsync(key, value);
    }
    public RedisValue GetKey(string key)
    {
        return Task.Run(async () => await _database.StringGetAsync(key)).Result;
    }
}
