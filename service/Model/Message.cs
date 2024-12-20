using Microsoft.OpenApi.Any;

namespace Service.Model;

public class KafkaProduceForm
{
    public required string Topic { get; set; }
    public required Message Message { get; set; }
}
public class Message
{
    public string? Key { get; set; }
    public string? Value { get; set; }
}

public class MessageResult
{
    public required string Topic { get; set; }
    public required int Partition { get; set; }
    public required long Offset { get; set; }
    public required Message Message { get; set; }
}