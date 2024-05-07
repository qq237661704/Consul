using Consul;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class ConsulBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsul(
            this IApplicationBuilder app
            , Action<ConsulServiceOptions>? optionsAction = null)
        {
            ConsulServiceOptions options;
            ILogger<ConsulServiceOptions> logger;
            using (var scope = app.ApplicationServices.CreateScope())
            {
                logger = scope.ServiceProvider.GetRequiredService<ILogger<ConsulServiceOptions>>();
                options = scope.ServiceProvider.GetRequiredService<IOptions<ConsulServiceOptions>>().Value;
                optionsAction?.Invoke(options);
            }

            if (string.IsNullOrEmpty(options.Address))
                return app;

            try
            {
                var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
                var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();

                if (!(app.Properties["server.Features"] is FeatureCollection features))
                {
                    return app;
                }

                // Register service with consul
                var registration = new AgentServiceRegistration()
                {
                    //Checks = new[] { httpCheck },
                    ID = Guid.NewGuid().ToString(),
                    Name = options.ServiceName,
                    Address = options.ServiceIP,
                    Port = options.ServicePort,
                    Tags = new[] { $"urlprefix-/{options.ServiceName}" } //添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
                };

                var result = consulClient.Agent.ServiceDeregister(registration.ID).Result;
                result = consulClient.Agent.ServiceRegister(registration).Result;

                applicationLifetime.ApplicationStopping.Register(() =>
                {
                    consulClient.Agent.ServiceDeregister(registration.ID).Wait();
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return app;
        }
    }
}
