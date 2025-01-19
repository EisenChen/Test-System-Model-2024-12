namespace Service.Controller;
using Microsoft.AspNetCore.Mvc;
using Service.Model;
using Service.Repositories;

[ApiController]
[Route("redis-test")]
public class RedisController(CacheContext cache) : ControllerBase
{    
    private readonly CacheContext _cache = cache;

    [HttpPost]
    public void CreateRedisRecord([FromBody] Redis? kv)
    {
        Console.WriteLine($"Here Comes: {kv?.Id},{kv?.Value}");
        _cache.SetKeyValue(kv?.Id??"1", kv?.Value);
        NoContent();
    }

    [HttpGet]
    public ActionResult<int> GetRedisRecord([FromQuery] string? Id)
    {
        Console.WriteLine($"Trying to get: {Id}");
        var res = _cache.GetValueByKey(Id??"1");
        return Ok(new Redis { Id = Id, Value = res });
    }
}

