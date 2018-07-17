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
                                response.AddLink(Rels.Password, "/password");
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

            routeBuilder.MapPutHandler<ChangePasswordRequest, ChangePasswordResponse>(
                "/password",
                async (context, changePasswordRequest) =>
                    await changePasswordRequest.Validate()
                        .Map(dto => dto.ToModel())
                        .BindAsync((PasswordChange model) => context.GetAuthenticatedLogin()
                            .AssertLogin()
                            .MapAsync((Login login) => Db.LoginById(appConfiguration.ConnectionString, login.LoginId))
                            .BindAsync(login => model.AssertOldPasswordMatches(login)
                                .RunAsync(m => Db.UpdateLoginPassword(appConfiguration.ConnectionString, login.LoginId, m.NewPasswordHash))
                            )
                        )
                        .Map((PasswordChange _) => new ChangePasswordResponse())
                );

            routeBuilder.MapPostHandler<AccountRequest, AccountResponse>(
                "/account",
                async (context, accountRequest) =>
                    await accountRequest.Validate()
                        .Bind(dto => context.GetAuthenticatedLogin()
                            .AssertLogin()
                            .Map(login => dto.ToModel(login))
                        )
                        .MapAsync(account => Db.InsertAccount(appConfiguration.ConnectionString, account))
                        .Map(account => account.ToDto())
                );
    
            routeBuilder.MapGetHandler<AccountResponseCollection>(
                "/account",
                async context =>
                    await context.GetAuthenticatedLogin()
                        .AssertLogin()
                        .MapAsync(login => Db.AccountByLogin(appConfiguration.ConnectionString, login))
                        .Map((IEnumerable<Account> accounts) => accounts.ToDto())
                );

            routeBuilder.MapDeleteHandler<AccountResponse>(
                "/account/{accountId}",
                async (context) =>
                    await context.GetIntegerRouteValue("accountId")
                        .BindAsync(accountId => Db.AccountById(appConfiguration.ConnectionString, accountId)
                            .ToTardisResult(HttpStatusCode.NotFound, "Not Found"))
                            .Bind(account => context.GetAuthenticatedLogin().AssertAccount(account))
                        .RunAsync((Account account) => Db.DeleteAccount(
                            appConfiguration.ConnectionString, 
                            new Account{ AccountId = account.AccountId }))
                        .Map(_ => new AccountResponse())
                );

            routeBuilder.MapPostHandler<TransactionRequest, TransactionResponse>(
                "/account/{accountId}/transaction",
                async (context, transactionRequest) =>
                    await context.GetIntegerRouteValue("accountId")
                        .BindAsync(accountId => Db.AccountById(appConfiguration.ConnectionString, accountId)
                            .ToTardisResult(HttpStatusCode.NotFound, "Not Found")
                            .Bind(account => context.GetAuthenticatedLogin().AssertAccount(account)))
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
                            .Bind(account => context.GetAuthenticatedLogin().AssertAccount(account))
                        .MapAsync(account => Db.TransactionsByAccount(appConfiguration.ConnectionString, account)
                        .Map(transactions => transactions.ToDto()))
                );

            routeBuilder.MapPostHandler<ScheduleRequest, ScheduleResponse>(
                "/account/{accountId}/schedule",
                async (context, scheduleRequest) =>
                    await context.GetIntegerRouteValue("accountId")
                        .BindAsync(accountId => Db.AccountById(appConfiguration.ConnectionString, accountId)
                            .ToTardisResult(HttpStatusCode.NotFound, "Not Found")
                            .Bind(account => context.GetAuthenticatedLogin().AssertAccount(account))
                            .Bind(account => scheduleRequest.Validate().Map(sr => sr.ToModel(account))))
                        .MapAsync(schedule => Db.InsertSchedule(appConfiguration.ConnectionString, schedule))
                        .Map(schedule => schedule.ToDto())
                );

            routeBuilder.MapGetHandler<ScheduleResponseCollection>(
                "/account/{accountId}/schedule",
                async context => 
                    await context.GetIntegerRouteValue("accountId")
                        .BindAsync(accountId => Db.AccountById(appConfiguration.ConnectionString, accountId)
                            .ToTardisResult(HttpStatusCode.NotFound, "Not Found"))
                            .Bind(account => context.GetAuthenticatedLogin().AssertAccount(account))
                        .MapAsync(account => Db.ScheduleByAccount(appConfiguration.ConnectionString, account)
                        .Map(schedules => schedules.ToDto()))
                );

            routeBuilder.MapDeleteHandler<ScheduleResponse>(
                "/account/{accountId}/schedule/{scheduleId}",
                async (context) =>
                    await context.GetIntegerRouteValue("accountId")
                        .BindAsync(accountId => Db.AccountById(appConfiguration.ConnectionString, accountId)
                            .ToTardisResult(HttpStatusCode.NotFound, "Not Found"))
                            .Bind(account => context.GetAuthenticatedLogin().AssertAccount(account))
                        .BindAsync<Account, TardisFault, int>(account => context.GetIntegerRouteValue("scheduleId")
                            .RunAsync((int scheduleId) => Db.DeleteSchedule(
                                appConfiguration.ConnectionString, 
                                new Schedule { ScheduleId = scheduleId, AccountId = account.AccountId })))
                        .Map(_ => new ScheduleResponse())
                );

            return routeBuilder;
        }
    }
}