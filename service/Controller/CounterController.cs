namespace Service.Controller;
using Microsoft.AspNetCore.Mvc;
using Service.Model;
using Service.Repository;

[ApiController]
[Route("counter")]
public class ConunterController(CounterContext context) : ControllerBase
{
    private readonly CounterContext _context = context;

    [HttpPost]
    public async Task<ActionResult> AddCounter([FromBody] Counter counter)
    {
        var dbCounter = await _context.Counters.FindAsync(1);

        if (dbCounter != null)
        {
            dbCounter.Value += counter.Value;
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
}

