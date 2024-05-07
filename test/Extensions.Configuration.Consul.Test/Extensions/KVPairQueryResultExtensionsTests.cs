using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using Consul;
using FluentAssertions;
using Moq;
using Winton.Extensions.Configuration.Consul.Parsers;
using Xunit;

namespace Winton.Extensions.Configuration.Consul.Extensions
{
    public class KVPairQueryResultExtensionsTests
    {
        public sealed class HasValue : KVPairQueryResultExtensionsTests
        {
            public static IEnumerable<object?[]> TestCases => new List<object?[]>
            {
                new object?[]
                {
                    null,
                    false
                },
                new object?[]
                {
                    new QueryResult<KVPair[]>
                    {
                        StatusCode = HttpStatusCode.NotFound
                    },
                    false
                },
                new object?[]
                {
                    new QueryResult<KVPair[]>
                    {
                        Response = null!,
                        StatusCode = HttpStatusCode.OK
                    },
                    false
                },
                new object?[]
                {
                    new QueryResult<KVPair[]>
                    {
                        Response = new KVPair[]
                        {
                        },
                        StatusCode = HttpStatusCode.OK
                    },
                    false
                },
                new object?[]
                {
                    new QueryResult<KVPair[]>
                    {
                        Response = new[]
                        {
                            new KVPair("Key")
                            {
                                Value = null
                            }
                        },
                        StatusCode = HttpStatusCode.OK
                    },
                    false
                },
                new object?[]
                {
                    new QueryResult<KVPair[]>
                    {
                        Response = new[]
                        {
                            new KVPair("Key/")
                        },
                        StatusCode = HttpStatusCode.OK
                    },
                    false
                },
                new object?[]
                {
                    new QueryResult<KVPair[]>
                    {
                        Response = new[]
                        {
                            new KVPair("Key")
                            {
                                Value = new byte[]
                                {
                                }
                            }
                        },
                        StatusCode = HttpStatusCode.OK
                    },
                    false
                },
                new object?[]
                {
                    new QueryResult<KVPair[]>
                    {
                        Response = new[]
                        {
                            new KVPair("Key")
                            {
                                Value = new byte[]
                                {
                                    1
                                }
                            }
                        },
                        StatusCode = HttpStatusCode.OK
                    },
                    true
                },
                new object?[]
                {
                    new QueryResult<KVPair[]>
                    {
                        Response = new[]
                        {
                            new KVPair("Key1")
                            {
                                Value = new byte[]
                                {
                                    1
                                }
                            },
                            new KVPair("Key2")
                        },
                        StatusCode = HttpStatusCode.OK
                    },
                    true
                },
                new object?[]
                {
                    new QueryResult<KVPair[]>
                    {
                        Response = new[]
                        {
                            new KVPair("Key1")
                            {
                                Value = new byte[]
                                {
                                    1
                                }
                            },
                            new KVPair("Key2/")
                        },
                        StatusCode = HttpStatusCode.OK
                    },
                    true
                }
            };

            [Theory]
            [MemberData(nameof(TestCases))]
            private void ShouldBeTrueWhenThereIsAtLeastOneLeafNodeWithSomeData(
                QueryResult<KVPair[]> queryResult,
                bool expected)
            {
                var hasValue = queryResult.HasValue();

                hasValue.Should().Be(expected);
            }
        }

        public class ToConfigDictionary : KVPairQueryResultExtensionsTests
        {
            [Fact]
            private void ShouldBeEmptyIfResponseIsNull()
            {
                var result = new QueryResult<KVPair[]>
                {
                    StatusCode = HttpStatusCode.OK
                };

                var config = result.ToConfigDictionary(_ => new Dictionary<string, string> { { "key", "value" } });

                config.Should().BeEmpty();
            }

            [Fact]
            private void ShouldNotParseIfConfigBytesIsNull()
            {
                var result = new QueryResult<KVPair[]>
                {
                    Response = new[]
                    {
                        new KVPair("path/test") { Value = new List<byte>().ToArray() }
                    },
                    StatusCode = HttpStatusCode.OK
                };

                var config = result.ToConfigDictionary(_ => new Dictionary<string, string> { { "key", "value" } });

                config.Should().BeEmpty();
            }

            [Theory]
            [InlineData("Key")]
            [InlineData("KEY")]
            [InlineData("key")]
            [InlineData("KeY")]
            private void ShouldParseIntoCaseInsensitiveDictionary(string key)
            {
                var result = new QueryResult<KVPair[]>
                {
                    Response = new[]
                    {
                        new KVPair("path/test") { Value = new List<byte> { 1 }.ToArray() }
                    },
                    StatusCode = HttpStatusCode.OK
                };

                var config = result.ToConfigDictionary(_ => new Dictionary<string, string> { { "kEy", "value" } });

                config.Should().ContainKey(key);
            }
        }
    }
}