using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using E247.Fun;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using TardisBank.Dto;

namespace TardisBank.Api
{
    public static class Infrastructure
    {
        public static IRouteBuilder MapGetHandler<T>(
            this IRouteBuilder routeBuilder,
            string template,
            Func<HttpContext, Task<Result<T, TardisFault>>> handler,
            bool authenticated = true)
            where T: IResponseModel
        {
            routeBuilder.MapVerbHandler("GET", template, handler, authenticated); 
            return routeBuilder;
        }

        public static IRouteBuilder MapDeleteHandler<T>(
            this IRouteBuilder routeBuilder,
            string template,
            Func<HttpContext, Task<Result<T, TardisFault>>> handler,
            bool authenticated = true)
            where T: IResponseModel
        {
            routeBuilder.MapVerbHandler("DELETE", template, handler, authenticated); 
            return routeBuilder;
        }

        public static IRouteBuilder MapPostHandler<TRequest, TResponse>(
            this IRouteBuilder routeBuilder,
            string template,
            Func<HttpContext, TRequest, Task<Result<TResponse, TardisFault>>> handler,
            bool authenticated = true)
            where TRequest: IRequestModel
            where TResponse: IResponseModel
        {
            routeBuilder.MapVerbWithRequestBodyHandler("POST", template, handler, authenticated);
            return routeBuilder;
        }

        public static IRouteBuilder MapPutHandler<TRequest, TResponse>(
            this IRouteBuilder routeBuilder,
            string template,
            Func<HttpContext, TRequest, Task<Result<TResponse, TardisFault>>> handler,
            bool authenticated = true)
            where TRequest: IRequestModel
            where TResponse: IResponseModel
        {
            routeBuilder.MapVerbWithRequestBodyHandler("PUT", template, handler, authenticated);
            return routeBuilder;
        }

        public static IRouteBuilder MapVerbWithRequestBodyHandler<TRequest, TResponse>(
            this IRouteBuilder routeBuilder,
            string verb,
            string template,
            Func<HttpContext, TRequest, Task<Result<TResponse, TardisFault>>> handler,
            bool authenticated = true)
            where TRequest: IRequestModel
            where TResponse: IResponseModel
        {
            return MapVerbHandler(
                routeBuilder,
                verb,
                template,
                async (HttpContext context) => 
                {
                    string requestBody = null;
                    using(var reader = new StreamReader(context.Request.Body))
                    {
                        requestBody = await reader.ReadToEndAsync();
                    }
                    var requestModel = JsonConvert.DeserializeObject<TRequest>(requestBody);
                    var result = await handler(context, requestModel);
                    return result;
                },
                authenticated
            );
        }

        public static IRouteBuilder MapVerbHandler<TResponse>(
            this IRouteBuilder routeBuilder,
            string verb,
            string template,
            Func<HttpContext, Task<Result<TResponse, TardisFault>>> handler,
            bool authenticated = true)
            where TResponse: IResponseModel
        {
            if(routeBuilder == null) throw new ArgumentNullException(nameof(routeBuilder));
            if(verb == null) throw new ArgumentNullException(nameof(verb));
            if(template == null) throw new ArgumentNullException(nameof(template));
            if(handler == null) throw new ArgumentNullException(nameof(handler));

            routeBuilder.MapVerb(verb, template, async context => 
            {
                if(authenticated && !context.IsAuthenticated())
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync(ErrorResponse("Forbidden"));
                    return;
                }

                var result = await handler(context);

                var json = result.Match(
                    Success: responseModel => {
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        responseModel.AddLink(Rels.Self, context.Request.Path);
                        responseModel.AddLink(Rels.Home, "/");
                        return JsonConvert.SerializeObject(responseModel);
                    },
                    Failure: x => 
                    {
                        context.Response.StatusCode = (int)x.HttpStatusCode;
                        return ErrorResponse(x.Message);
                    });

                context.Response.Headers.Add("Content-Type", new StringValues("application/json"));
                await context.Response.WriteAsync(json);
            });
            return routeBuilder;
        }

        public static string ErrorResponse(string message)
            => JsonConvert.SerializeObject(new ErrorResponse
            {
                Message = message
            });

        public static Result<int, TardisFault> GetIntegerRouteValue(this HttpContext context, string key)
        {
            var value = context.GetRouteValue(key) as string;
            if(value != null && int.TryParse(value, out int intValue))
            {
                return intValue;
            }
            return new TardisFault(HttpStatusCode.NotFound, "Not Found");
        }

        public static Result<string, TardisFault> GetStringRouteValue(this HttpContext context, string key)
        {
            var value = context.GetRouteValue(key) as string;
            if(string.IsNullOrEmpty(value))
            {
                return new TardisFault(HttpStatusCode.NotFound, "Not Found");
            }
            return value;
        }
    }
}