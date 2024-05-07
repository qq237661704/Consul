using Consul;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddConsul(this IServiceCollection service,
            IConfiguration configuration)
        {
            var url = configuration["AbpConsul:Address"];
            if (string.IsNullOrEmpty(url))
                return service;

            service.Configure<ConsulServiceOptions>(configuration.GetSection("AbpConsul"));

            service.AddSingleton<IConsulClient, ConsulClient>(
                p => new ConsulClient(x =>
                {
                    x.Address = new Uri(url!);
                }));

            return service;
        }
    }
}