namespace Service.Controller;

using Microsoft.AspNetCore.Mvc;
using Service.Repositories;
using Service.Model;
using System.Text.Json;

[ApiController]
[Route("kafka")]
public class KafkaController(KafkaBackGroundService kafkaService) : ControllerBase
{
    private readonly KafkaBackGroundService _kafkaService = kafkaService;

    [HttpPost("message")]
    public void SetValueByTopic([FromBody] KafkaProduceForm message)
    {
        if (message.Topic == null || message.Message == null || message.Message.Value == null) return;
        try
        {
            _kafkaService.ProduceMessage(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    public class TopicRequest
    {
        public required string Topic { get; set; }
    }
    [HttpGet("message")]
    public ActionResult<MessageResult> GetValueByTopic([FromQuery] TopicRequest req)
    {
        try
        {
            var res = _kafkaService.GetValueByTopicAsync(req.Topic);
            return Ok(res);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return StatusCode(500);
    }

    [HttpGet("test")]
    public ActionResult GetTestResult()
    {
        return Ok("Good");
    }
}