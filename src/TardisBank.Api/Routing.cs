using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TardisBank.Dto;

namespace TardisBank.Api
{
    public static class Routing
    {
        public static RouteBuilder CreateRoutes(this RouteBuilder routeBuilder)
        {
            routeBuilder.MapGetHandler("/", context => 
                {
                    return Task.FromResult(new HomeUnauthenticatedResponse());
                });

            routeBuilder.MapPostHandler<RegisterReqeust, RegisterResponse>(
                "/register", 
                (context, registerRequest) => 
                {
                    return Task.FromResult(new RegisterResponse());
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