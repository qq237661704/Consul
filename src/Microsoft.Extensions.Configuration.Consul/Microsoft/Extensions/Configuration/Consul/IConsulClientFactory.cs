
using Consul;
using Microsoft;
using Microsoft.Extensions.Configuration.Consul;

namespace Microsoft.Extensions.Configuration.Consul
{
    /// <summary>A factory responsible for creating <see cref="IConsulClient" /> objects.</summary>
    internal interface IConsulClientFactory
    {
        /// <summary>Creates a new instance of an <see cref="IConsulClient" />.</summary>
        /// <returns>A new <see cref="IConsulClient" />.</returns>
        IConsulClient Create();
    }
}