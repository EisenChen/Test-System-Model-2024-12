namespace Service.Repositories;

using System.Text.Json;
using Confluent.Kafka;
using Service.Model;

public class KafkaBackGroundService(IProducer<string, string> producer, IConsumer<string, string> consumer, CacheContext cache) : BackgroundService
{
    private readonly IProducer<string, string> _producer = producer;
    private readonly IConsumer<string, string> _consumer = consumer;
    private readonly CacheContext _cache = cache;
    private bool _subscribed = false;
    private readonly string _initTopic = "waiting-room";
    public Task<Task> ProduceMessage(KafkaProduceForm message)
    {

        if (message.Topic == null || message.Message.Value == null) return Task.FromResult(Task.CompletedTask);
        message.Message.Key = _cache.GetListRangeByKey(message.Topic).ToString();
        return Task.FromResult(Task.Run(async () =>
        {
            _ = await _producer.ProduceAsync
            (
                message.Topic,
                new Message<string, string>
                {
                    Key = message.Message.Key,
                    Value = message.Message.Value
                }
            );
        }));
    }
    public string? GetValueByTopicAsync(string topic)
    {
        SetSubscribe(topic);
        return _cache.GetListByKey(topic);
    }

    private async Task<Task> CreateTopic(string topic)
    {
        var initData = new KafkaProduceForm
        {
            Topic = topic,
            Message = new Message
            {
                Key = "1",
                Value = "1"
            }
        };
        return await ProduceMessage(initData);
    }
    private async void SetSubscribe(string topic)
    {
        try
        {
            _consumer.Subscribe(topic);
        }
        catch (ConsumeException ex)
        {
            Console.WriteLine($"Consume Error:{ex}");
            Console.WriteLine("Try ReSub!");
            await CreateTopic(topic);
            SetSubscribe(topic);
        }
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Task.Yield() save the ExecuteAsync and make it work correctly.
        await Task.Yield();

        if (!_subscribed)
        {
            SetSubscribe(_initTopic);
            _subscribed = true;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = _consumer.Consume(stoppingToken);
                if (result != null)
                {

                    var jsonData = JsonSerializer.Serialize(
                        new MessageResult
                        {
                            Topic = result.Topic,
                            Partition = result.Partition.Value,
                            Offset = result.Offset.Value,
                            Message = new Message
                            {
                                Key = result.Message.Key,
                                Value = result.Message.Value
                            }
                        }
                    );
                    _cache.AppendValueByKey(result.Topic, jsonData);
                }
            }
            catch (ConsumeException ex)
            {
                Console.WriteLine($"Kafka Error At process: {ex.Error.Reason}");
            }
        }
    }

    public override void Dispose()
    {
        _producer.Dispose();
        _consumer.Close();
        _consumer.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}


