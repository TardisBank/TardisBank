using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

public static class Routing
{
    public static RouteBuilder CreateRoutes(this RouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/", context => 
        {
            return context.Response.WriteAsync("Hello World!");
        });

        routeBuilder.MapGet("/{name}", context => 
        {
            var name = (string)context.GetRouteValue("name");
            return context.Response.WriteAsync($"Hello {name}!");
        });

        return routeBuilder;
    }
}