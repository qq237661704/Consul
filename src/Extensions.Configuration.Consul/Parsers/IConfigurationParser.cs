
using System.Collections.Generic;
using System.IO;

namespace Extensions.Configuration.Consul.Parsers
{
    /// <summary>
    ///     Defines how the configuration loaded from Consul should be parsed.
    /// </summary>
    public interface IConfigurationParser
    {
        /// <summary>
        ///     Parse the <see cref="Stream" /> into a dictionary.
        /// </summary>
        /// <param name="stream">The stream to parse.</param>
        /// <returns>A dictionary representing the configuration in a flattened form.</returns>
        IDictionary<string, string> Parse(Stream stream);
    }
}