using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace TardisBank.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var appConfiguration = AppConfiguration.LoadFromEnvironment();

            app.UseErrorHandler(env.IsDevelopment());

            app.Use(Authentication.Authenticate(
                token => Authentication.DecryptToken(
                    appConfiguration.EncryptionKey, 
                    () => DateTimeOffset.Now, 
                    token)));

            app.UseRouter(new RouteBuilder(app).CreateRoutes(appConfiguration).Build());
        }
    }
}
