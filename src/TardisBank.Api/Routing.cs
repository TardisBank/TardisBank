using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TardisBank.Api
{
    public static class Routing
    {
        public static RouteBuilder CreateRoutes(this RouteBuilder routeBuilder)
        {
            routeBuilder.MapGetHandler("/", context => 
            {
                return Task.FromResult(new HomeUnauthenticatedResponseModel());
            });

            routeBuilder.MapGet("/{name}", context => 
            {
                var name = (string)context.GetRouteValue("name");
                return context.Response.WriteAsync($"Hello {name}!");
            });

            return routeBuilder;
        }
    }
}