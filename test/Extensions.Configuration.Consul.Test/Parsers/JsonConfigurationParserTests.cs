using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Extensions.Configuration.Consul.Parsers
{
    public class JsonConfigurationParserTests
    {
        private readonly JsonConfigurationParser _parser;

        public JsonConfigurationParserTests()
        {
            _parser = new JsonConfigurationParser();
        }

        public sealed class Parse : JsonConfigurationParserTests
        {
            public static IEnumerable<object[]> TestCases => new List<object[]>
            {
                new object[]
                {
                    "{\"Key\": \"Value\"}",
                    new Dictionary<string, string> { { "Key", "Value" } }
                },
                new object[]
                {
                    "{\"parent\": {\"child\": \"Value\"} }",
                    new Dictionary<string, string?> { { "parent:child", "Value" } }
                },
                new object[]
                {
                    "{\"server\": {\"ip\": \"192.168.0.1\", \"port\": 5000} }",
                    new Dictionary<string, string?> { { "server:ip", "192.168.0.1" }, { "server:port", "5000" } }
                },
                new object[]
                {
                    "{\"Key/WithSlash\": \"Value\"}",
                    new Dictionary<string, string> { { "Key/WithSlash", "Value" } }
                }
            };

            [Theory]
            [MemberData(nameof(TestCases))]
            private void ShouldParseSimpleJsonFromStream(string json, IDictionary<string, string?> expected)
            {
                using Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
                var result = _parser.Parse(stream);

                result.Should().BeEquivalentTo(expected);
            }
        }
    }
}