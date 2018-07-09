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
            Func<HttpContext, Task<T>> handler)
            where T: IResponseModel
        {
            routeBuilder.MapGet(template, async context => 
            {
                var responseModel = await handler(context);
                responseModel.AddLink(Rels.Self, context.Request.Path);
                responseModel.AddLink(Rels.Home, "/");
                var json = JsonConvert.SerializeObject(responseModel);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.Headers.Add("Content-Type", new StringValues("application/json"));
                await context.Response.WriteAsync(json);
            });
            return routeBuilder;
        }

        public static IRouteBuilder MapPostHandler<TRequest, TResponse>(
            this IRouteBuilder routeBuilder,
            string template,
            Func<HttpContext, TRequest, Task<Result<TResponse, TardisFault>>> handler)
            where TRequest: IRequestModel
            where TResponse: IResponseModel
        {
            routeBuilder.MapPost(template, async context => 
            {
                string requestBody = null;
                using(var reader = new StreamReader(context.Request.Body))
                {
                    requestBody = await reader.ReadToEndAsync();
                }
                var requestModel = JsonConvert.DeserializeObject<TRequest>(requestBody);
                var result = await handler(context, requestModel);

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
                        return $"{{ \"Message\": \"{x.Message}\" }}";
                    });

                context.Response.Headers.Add("Content-Type", new StringValues("application/json"));
                await context.Response.WriteAsync(json);
            });
            return routeBuilder;
        }
    }
}