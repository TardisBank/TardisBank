using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

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
                responseModel.AddLink("self", context.Request.Path);
                var json = JsonConvert.SerializeObject(responseModel);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.Headers.Add("Content-Type", new StringValues("application/json"));
                await context.Response.WriteAsync(json);
            });
            return routeBuilder;
        }
    }
}