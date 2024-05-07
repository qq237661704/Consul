 
using System.Collections.Generic;
using System.IO;

namespace Extensions.Configuration.Consul.Parsers
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