using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TardisBank.Dto;
using E247.Fun;
using System;
using static E247.Fun.Result<TardisBank.Api.Login, TardisBank.Api.TardisFault>;

namespace TardisBank.Api
{
    public static class Routing
    {
        public static RouteBuilder CreateRoutes(this RouteBuilder routeBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable("TARDISBANK_DB_CON");
            var encryptionKey = Environment.GetEnvironmentVariable("TARDISBANK_KEY");

            routeBuilder.MapGetHandler("/", context => 
                {
                    var response = new HomeUnauthenticatedResponse();
                    response.AddLink(Rels.Login, "/login");
                    return Task.FromResult(response);
                });

            routeBuilder.MapPostHandler<RegisterRequest, RegisterResponse>(
                "/", 
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

            routeBuilder.MapPostHandler<LoginRequest, LoginResponse>(
                "/login", 
                async (context, loginRequest) => 
                    await loginRequest.Validate()
                        .BindAsync(dto => Db.LoginByEmail(connectionString, dto.Email)
                            .Match(
                                Some: login => Succeed(login),
                                None: () => Fail(new TardisFault("Unknown Email or Password."))
                            )
                        )
                        .Bind(login => Password.HashMatches(loginRequest.Password, login.PasswordHash)
                            ? Succeed(login)
                            : Fail(new TardisFault("Unknown Email or Password.")))
                        .Map(login => Authentication.CreateToken(encryptionKey, () => DateTimeOffset.Now, login))
                        .Map(token => new LoginResponse { Token = token })
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