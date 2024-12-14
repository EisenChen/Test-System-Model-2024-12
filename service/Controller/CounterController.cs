namespace Service.Controller;
using Microsoft.AspNetCore.Mvc;
using service;
using Service.Model;
using Service.Repository;

[ApiController]
[Route("counter")]
public class ConunterController(CounterContext context, CacheContext cache) : ControllerBase
{
    private readonly CounterContext _context = context;
    private readonly CacheContext _cache = cache;

    [HttpPost]
    public async Task<ActionResult> AddCounter([FromBody] Counter counter)
    {
        var dbCounter = await _context.Counters.FindAsync(1);

        if (dbCounter != null)
        {
            dbCounter.Value += counter.Value;
            SetCounterCache(dbCounter.Value);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        else
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    public async Task<ActionResult<int>> GetCounter()
    {
        var res = await _context.Counters.FindAsync(1);
        return Ok(res);
    }

    [HttpGet("cache")]
    public ActionResult<int> GetCounterCache()
    {
        int res = (int)_cache.GetKey("counter");
        return Ok(new Counter { Value = res });
    }

    public void SetCounterCache(int value)
    {
        if (value % 2 == 0)
        {
            _cache.Set("counter", value);
        }
    }
}

