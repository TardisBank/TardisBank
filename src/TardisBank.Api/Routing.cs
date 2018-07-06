using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TardisBank.Dto;
using E247.Fun;
using System;

namespace TardisBank.Api
{
    public static class Routing
    {
        public static RouteBuilder CreateRoutes(this RouteBuilder routeBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable("TARDISBANK_DB_CON");

            routeBuilder.MapGetHandler("/", context => 
                {
                    var response = new HomeUnauthenticatedResponse();
                    response.AddLink(Rels.Register, "/register");
                    return Task.FromResult(response);
                });

            routeBuilder.MapPostHandler<RegisterRequest, RegisterResponse>(
                "/register", 
                async (context, registerRequest) => 
                    await registerRequest.Validate()
                        .BindAsync(dto => Db.LoginByEmail(connectionString, dto.Email)
                            .Match(
                                Some: _ => Result<RegisterRequest, TardisFault>.Fail(
                                    new TardisFault("Email already exists")),
                                None: () => Result<RegisterRequest, TardisFault>.Succeed(dto)
                            )
                        )
                        .Map(dto => dto.ToModel())
                        .MapAsync(login => Db.InsertLogin(connectionString, login))
                        .Map(login => login.ToDto())
                );

            routeBuilder.MapGet("/{name}", context => 
                {
                    var name = (string)context.GetRouteValue("name");
                    return context.Response.WriteAsync($"Hello {name}!");
                });

            return routeBuilder;
        }
    }
}