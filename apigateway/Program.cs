using apigateway;

var builder = WebApplication.CreateBuilder(args);

var SERVICE_URL = Environment.GetEnvironmentVariable("SERVICE_URL");

Console.WriteLine();
Console.WriteLine("Service URL:${0}", SERVICE_URL);
Console.WriteLine();


var proxyConfigProvider = new ConfigProvider();

proxyConfigProvider.SetClusterUrl(SERVICE_URL != null ? SERVICE_URL : "http://localhost:5011/");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4561").AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddReverseProxy().LoadFromMemory(proxyConfigProvider.GetRoutes(), proxyConfigProvider.GetClusters());



var app = builder.Build();

app.UseCors();
app.MapReverseProxy();

app.Run();
