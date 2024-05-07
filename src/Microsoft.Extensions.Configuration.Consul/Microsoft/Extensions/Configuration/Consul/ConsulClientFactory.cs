

using Consul;
using Microsoft;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Consul;
 
namespace Microsoft.Extensions.Configuration.Consul
{
    internal sealed class ConsulClientFactory : IConsulClientFactory
    {
        private readonly IConsulConfigurationSource _consulConfigSource;

        public ConsulClientFactory(IConsulConfigurationSource consulConfigSource)
        {
            _consulConfigSource = consulConfigSource;
        }

        public IConsulClient Create()
        {
            return new ConsulClient(
                _consulConfigSource.ConsulConfigurationOptions,
                _consulConfigSource.ConsulHttpClientOptions,
                _consulConfigSource.ConsulHttpClientHandlerOptions);
        }
    }
}