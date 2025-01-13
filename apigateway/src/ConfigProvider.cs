using Yarp.ReverseProxy.Configuration;

namespace apigateway;

public class ConfigProvider
{
    private string _url;
    public ConfigProvider()
    {
        _url = "";
    }

    public RouteConfig[] GetRoutes()
    {
        return [
            new RouteConfig()
            {
                RouteId = "route1",
                ClusterId = "cluster1",
                Match = new RouteMatch
                {
                    Path = "{**catch-all}"
                }
            }
        ];
    }

    public void SetClusterUrl(string url)
    {
        _url = url;
    }

    public ClusterConfig[] GetClusters()
    {
        return [
            new ClusterConfig(){
                ClusterId = "cluster1",
                Destinations = new Dictionary<string,DestinationConfig>()
                {
                     {"destination1", new DestinationConfig(){Address=_url}}
                }
            }
        ];
    }
}