using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using TardisBank.Dto;

namespace TardisBank.Api
{
    public static class ErrorHandler
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app, bool includeExceptionDetails) 
        {
            app.Use(async (context, next) => 
            {
                try
                {
                    await next();
                }
                catch(Exception exception)
                {
                    if(context.Response.HasStarted)
                    {
                        throw;
                    }

                    var message = includeExceptionDetails
                        ? exception.ToString()
                        : "Internal Server Error";

                    context.Response.Clear();
                    context.Response.StatusCode = 500;
                    context.Response.Headers.Add("Content-Type", new StringValues("application/json"));

                    var json = JsonConvert.SerializeObject(new ErrorResponse
                    {
                        Message = message
                    });

                    await context.Response.WriteAsync(json);
                }
            });
            return app;
        }
    }
}