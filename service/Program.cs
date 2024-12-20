using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Service.Repositories;


var builder = WebApplication.CreateBuilder(args);

var REDIS_URL = Environment.GetEnvironmentVariable("REDIS_URL") ?? "localhost:4564";
var KAFKA_URL = Environment.GetEnvironmentVariable("KAFKA_URL") ?? "localhost:9092";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =============================
//     SQL Server
// =============================
builder.Services.AddDbContext<CounterContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// =============================
//     Redis
// =============================
builder.Services.AddSingleton<CacheContext>(
    options => new CacheContext(REDIS_URL)
    );

// =============================
//     Kafka
// =============================
// Console.WriteLine("Kafka URL:{0}", KAFKA_URL);
builder.Services.AddSingleton<IProducer<string, string>>(new ProducerBuilder<string, string>(new ProducerConfig { BootstrapServers = KAFKA_URL }).Build());
builder.Services.AddSingleton<IConsumer<string, string>>(new ConsumerBuilder<string, string>(new ConsumerConfig { BootstrapServers = KAFKA_URL, GroupId = "kafka-client-02", AutoOffsetReset = AutoOffsetReset.Earliest }).Build());
builder.Services.AddSingleton<KafkaBackGroundService>();
builder.Services.AddHostedService<KafkaBackGroundService>();

// =============================
//     Controller
// =============================
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();