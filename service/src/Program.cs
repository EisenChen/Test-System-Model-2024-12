using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Service.Repositories;


var builder = WebApplication.CreateBuilder(args);

var REDIS_URL = Environment.GetEnvironmentVariable("REDIS_URL") ?? "localhost:21001";
var KAFKA_URL = Environment.GetEnvironmentVariable("KAFKA_URL") ?? "localhost:21002";
// var CORS_ORIGIN = Environment.GetEnvironmentVariable("CORS_ORIGIN") ??"http://localhost:5173";

var SQL_SERVER = Environment.GetEnvironmentVariable("SQL_SERVER") ?? null;
var SQL_SERVER_PORT = Environment.GetEnvironmentVariable("SQL_SERVER_PORT") ?? null;
var SQL_DATABASE = Environment.GetEnvironmentVariable("SQL_DATABASE") ?? null;
var SQL_USER = Environment.GetEnvironmentVariable("SQL_USER") ?? null;
var SQL_PASSWORD = Environment.GetEnvironmentVariable("SQL_PASSWORD") ?? null;

var connectionString = $"Server={SQL_SERVER},{SQL_SERVER_PORT};Database={SQL_DATABASE};User Id={SQL_USER};Password={SQL_PASSWORD};TrustServerCertificate=true;";
if(SQL_SERVER==null||SQL_SERVER_PORT==null||SQL_DATABASE==null||SQL_USER==null||SQL_PASSWORD==null){
    connectionString=builder.Configuration.GetConnectionString("DefaultConnection");
}
Console.WriteLine($"Connection String is:{connectionString}");
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =============================
//     Controller
// =============================
builder.Services.AddControllers();

// =============================
//     SQL Server
// =============================
builder.Services.AddDbContext<CounterContext>(
    options => options.UseSqlServer(connectionString)
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
builder.Services.AddSingleton<IProducer<string, string>>(new ProducerBuilder<string, string>(new ProducerConfig { BootstrapServers = KAFKA_URL }).Build());
builder.Services.AddSingleton<IConsumer<string, string>>(new ConsumerBuilder<string, string>(new ConsumerConfig { BootstrapServers = KAFKA_URL, GroupId = "kafka-client-02", AutoOffsetReset = AutoOffsetReset.Earliest }).Build());
builder.Services.AddSingleton<KafkaBackGroundService>();
builder.Services.AddHostedService<KafkaBackGroundService>();


// builder.Services.AddCors(options =>
// {
//     options.AddDefaultPolicy(builder =>
//     {
//         // builder.WithOrigins("http://localhost:20001").WithMethods("POST","GET","OPTIONS").AllowAnyHeader();                
//         // builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();        
//         builder.WithOrigins(CORS_ORIGIN).AllowAnyMethod().AllowAnyHeader();
//     });
// });


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CounterContext>();
    context.Database.Migrate();  
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// =============================
//     Promethus
// =============================
app.UseHttpMetrics();
app.MapMetrics();

app.UseCors();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();