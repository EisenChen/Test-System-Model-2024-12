using apigateway;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

var SERVICE_URL = Environment.GetEnvironmentVariable("SERVICE_URL") ?? "http://localhost:5011/";
var KAFKA_URL = Environment.GetEnvironmentVariable("KAFKA_URL") ?? "http://localhost:9092/";
var CORS_ORIGIN = Environment.GetEnvironmentVariable("CORS_ORIGIN") ??"http://localhost:5173";

// Yarp ///////////////////////////////////////////////////////
var proxyConfigProvider = new ConfigProvider();

proxyConfigProvider.SetClusterUrl(SERVICE_URL);
Console.WriteLine($"CORS_ORIGIN is:{CORS_ORIGIN}");
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        // builder.WithOrigins(CORS_ORIGIN).WithMethods("POST","GET","OPTIONS").AllowAnyHeader();
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddReverseProxy().LoadFromMemory(proxyConfigProvider.GetRoutes(), proxyConfigProvider.GetClusters());
// Kafka ///////////////////////////////////////////////////////
builder.AddKafkaProducer<string, string>(KAFKA_URL);

var app = builder.Build();

// =============================
//     Promethus
// =============================
app.UseHttpMetrics();
app.MapMetrics();

app.UseCors();
app.MapReverseProxy();

app.Run();
