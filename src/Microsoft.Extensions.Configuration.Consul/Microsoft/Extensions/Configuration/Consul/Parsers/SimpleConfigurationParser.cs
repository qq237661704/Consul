
using System.Collections.Generic;
using System.IO;
using Microsoft;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Consul;
using Microsoft.Extensions.Configuration.Consul.Parsers;

namespace Microsoft.Extensions.Configuration.Consul.Parsers
{
    /// <inheritdoc />
    /// <summary>
    ///     Implementation of <see cref="IConfigurationParser" /> for parsing simple values.
    /// </summary>
    public sealed class SimpleConfigurationParser : IConfigurationParser
    {
        /// <inheritdoc />
        public IDictionary<string, string> Parse(Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            return new Dictionary<string, string> { { string.Empty, streamReader.ReadToEnd() } };
        }
    }
}