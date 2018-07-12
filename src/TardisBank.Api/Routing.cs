using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TardisBank.Dto;
using E247.Fun;
using System;
using static E247.Fun.Result<TardisBank.Api.Login, TardisBank.Api.TardisFault>;
using System.Net;
using System.Collections.Generic;

namespace TardisBank.Api
{
    public static class Routing
    {
        public static RouteBuilder CreateRoutes(
            this RouteBuilder routeBuilder,
            AppConfiguration appConfiguration)
        {
            routeBuilder.MapGetHandler("/", context => 
                {
                    var response = new HomeResponse();
                    context.GetAuthenticatedLogin()
                        .Match(
                            Some: login => 
                            { 
                                response.Email = login.Email;
                                response.AddLink(Rels.Account, "/account");
                            },
                            None: () => response.AddLink(Rels.Login, "/login")
                        );
                    return Task.FromResult(Result<HomeResponse, TardisFault>.Succeed(response));
                },
                authenticated: false);

            routeBuilder.MapDeleteHandler<HomeResponse>("/", context => 
                    context.GetAuthenticatedLogin()
                        .MatchAsync(
                            Some: async login => 
                            { 
                                await Db.DeleteLogin(appConfiguration.ConnectionString, login);
                                return (Result<HomeResponse, TardisFault>)new HomeResponse();
                            },
                            None: () => new HomeResponse()
                        )
                );

            routeBuilder.MapPostHandler<RegisterRequest, RegisterResponse>(
                "/", 
                async (context, registerRequest) => 
                    await registerRequest.Validate()
                        .BindAsync(dto => Db.LoginByEmail(appConfiguration.ConnectionString, dto.Email)
                            .Match(
                                Some: _ => Result<RegisterRequest, TardisFault>.Fail(
                                    new TardisFault("Email already exists")),
                                None: () => Result<RegisterRequest, TardisFault>.Succeed(dto)
                            )
                        )
                        .Map(dto => dto.ToModel())
                        .MapAsync(login => Db.InsertLogin(appConfiguration.ConnectionString, login))
                        .Map(login => login.ToDto()),
                authenticated: false);

            routeBuilder.MapPostHandler<LoginRequest, LoginResponse>(
                "/login", 
                async (context, loginRequest) => 
                    await loginRequest.Validate()
                        .BindAsync(dto => Db.LoginByEmail(appConfiguration.ConnectionString, dto.Email)
                            .ToTardisResult(HttpStatusCode.InternalServerError, "Unknown Email or Password."))
                        .Bind(login => Password.HashMatches(loginRequest.Password, login.PasswordHash)
                            ? Succeed(login)
                            : Fail(new TardisFault("Unknown Email or Password.")))
                        .Map(login => Authentication.CreateToken(appConfiguration.EncryptionKey, () => DateTimeOffset.Now, login))
                        .Map(token => new LoginResponse { Token = token }),
                authenticated: false);

            routeBuilder.MapPostHandler<AccountRequest, AccountResponse>(
                "/account",
                async (context, accountRequest) =>
                    await accountRequest.Validate()
                        .Bind(dto => context.GetAuthenticatedLogin()
                            .ToTardisResult(HttpStatusCode.InternalServerError, "Expected auth token not present")
                            .Map(login => dto.ToModel(login))
                        )
                        .MapAsync(account => Db.InsertAccount(appConfiguration.ConnectionString, account))
                        .Map(account => account.ToDto())
                );
    
            routeBuilder.MapGetHandler<AccountResponseCollection>(
                "/account",
                async context =>
                    await context.GetAuthenticatedLogin()
                        .ToTardisResult(HttpStatusCode.InternalServerError, "Expected auth token not present")
                        .MapAsync(login => Db.AccountByLogin(appConfiguration.ConnectionString, login))
                        .Map((IEnumerable<Account> accounts) => accounts.ToDto())
                );

            routeBuilder.MapDeleteHandler<AccountResponse>(
                "/account/{accountId}",
                async (context) =>
                    await context.GetIntegerRouteValue("accountId")
                        .RunAsync(accountId => Db.DeleteAccount(appConfiguration.ConnectionString, new Account{ AccountId = accountId }))
                        .Map((int _) => new AccountResponse())
                );

            routeBuilder.MapPostHandler<TransactionRequest, TransactionResponse>(
                "/account/{accountId}/transaction",
                async (context, transactionRequest) =>
                    await context.GetIntegerRouteValue("accountId")
                        .BindAsync(accountId => Db.AccountById(appConfiguration.ConnectionString, accountId)
                            .ToTardisResult(HttpStatusCode.NotFound, "Not Found"))
                        .MapAsync(account => Db.TransactionsByAccount(appConfiguration.ConnectionString, account)
                            .Map(transactions => transactions.FirstOrDefault() ?? new Transaction())
                            .Map(transaction => transactionRequest.ToModel(account, transaction, DateTimeOffset.Now)))
                        .MapAsync(transaction => Db.InsertTransaction(appConfiguration.ConnectionString, transaction))
                        .Map(transaction => transaction.ToDto())
                );

            routeBuilder.MapGetHandler<TransactionResponseCollection>(
                "/account/{accountId}/transaction",
                async context => 
                    await context.GetIntegerRouteValue("accountId")
                        .BindAsync(accountId => Db.AccountById(appConfiguration.ConnectionString, accountId)
                            .ToTardisResult(HttpStatusCode.NotFound, "Not Found"))
                        .MapAsync(account => Db.TransactionsByAccount(appConfiguration.ConnectionString, account)
                        .Map(transactions => transactions.ToDto()))
                );

            return routeBuilder;
        }
    }
}