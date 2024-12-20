using apigateway;

var builder = WebApplication.CreateBuilder(args);

var SERVICE_URL = Environment.GetEnvironmentVariable("SERVICE_URL") ?? "http://localhost:5011/";
var KAFKA_URL = Environment.GetEnvironmentVariable("KAFKA_URL") ?? "http://localhost:5011/";

// Yarp ///////////////////////////////////////////////////////
var proxyConfigProvider = new ConfigProvider();

proxyConfigProvider.SetClusterUrl(SERVICE_URL);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4561").AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddReverseProxy().LoadFromMemory(proxyConfigProvider.GetRoutes(), proxyConfigProvider.GetClusters());
// Kafka ///////////////////////////////////////////////////////
builder.AddKafkaProducer<string, string>(KAFKA_URL);

var app = builder.Build();

app.UseCors();
app.MapReverseProxy();

app.Run();
