namespace Service.Repositories;

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using Service.Model;
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

    public void SetKeyValue(string key, RedisValue value)
    {
        _database.StringSetAsync(key, value);
    }
    public RedisValue GetValueByKey(string key)
    {
        return Task.Run(async () => await _database.StringGetAsync(key)).Result;
    }
    public string GetListByKey(string key)
    {
        var values = Task.Run(async () => await _database.ListRangeAsync(key, 0, -1)).Result;
        var stringValues = Array.ConvertAll(values, value => value.ToString());
        return JsonSerializer.Serialize(stringValues);
    }
    public long GetListRangeByKey(string key)
    {
        var values = Task.Run(async () => await _database.ListLengthAsync(key)).Result;
        return values;
    }
    public void AppendValueByKey(string key, RedisValue value)
    {
        if (!_database.KeyExists(key))
        {
            _database.ListLeftPushAsync(key, "");
        }
        _database.ListLeftPushAsync(key, value);
    }
}
