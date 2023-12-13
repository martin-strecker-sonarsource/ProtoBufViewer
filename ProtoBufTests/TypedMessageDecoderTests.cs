using FluentAssertions;
using Google.Protobuf;
using ProtoBuf.Logic;

namespace ProtoBufTests
{
    [TestClass]
    [DeploymentItem(@"Artefacts\AnalyzerReport.proto")]
    [DeploymentItem(@"Artefacts\token-type.pb")]
    [DeploymentItem(@"Artefacts\symrefs.pb")]
    [DeploymentItem(@"Artefacts\file-metadata.pb")]
    [DeploymentItem(@"Artefacts\log.pb")]
    [DeploymentItem(@"Artefacts\metrics.pb")]
    public class TypedMessageDecoderTests
    {
        [TestMethod]
        public void TypedMessageDecoder_TokenType()
        {
            IReadOnlyList<TypedMessage> actual = ParseBinary("token-type.pb", "TokenTypeInfo");
            var tokenType = actual.Should().ContainSingle().Which;
            tokenType.Should().BeEquivalentTo(new { MessageType = "TokenTypeInfo", Type = "Message" });
            tokenType.Fields.Should().HaveCount(127);
            tokenType.Fields[0].Should().BeEquivalentTo(new
            {
                Index = 1,
                Name = "file_path",
                Value = new
                {
                    Type = "String",
                    Value = @"C:\Projects\Sprints\UtilityAnalyzerPerf\Benchmark\Projects\fluentassertions\Tests\Approval.Tests\ApiApproval.cs"
                }
            });
            tokenType.Fields[1].Should().BeEquivalentTo(new
            {
                Index = 2,
                Name = "token_info",
                Value = new
                {
                    Type = "Message",
                    Fields = new[] {
                        new { Index= 1, Name= "token_type", Value = new { Type = "Enum" } },
                        new { Index= 2, Name= "text_range" , Value = new { Type = "Message" }},
                    },
                }
            });
        }

        [TestMethod]
        public void TypedMessageDecoder_SymbolReferenceInfo()
        {
            IReadOnlyList<TypedMessage> actual = ParseBinary("symrefs.pb", "SymbolReferenceInfo");
            actual.Should().HaveCount(2);
            var symRef = actual[0];
            symRef.Should().BeEquivalentTo(new
            {
                MessageType = "SymbolReferenceInfo",
                Type = "Message",
                Fields = new[]
                {
                    new
                    {
                        Name = "file_path",
                        Value = (object)new { Value = @"C:\Projects\Sprints\UtilityAnalyzerPerf\Benchmark\Projects\fluentassertions\Tests\AssemblyB\ClassB.cs" },
                    },
                    new
                    {
                        Name = "reference",
                        Value = (object)new
                        {
                            Fields = new[]
                            {
                                new
                                {
                                    Name = "declaration",
                                    Value = new
                                    {
                                        Fields = new[]
                                        {
                                            new { Name = "start_line" },
                                            new { Name = "end_line" },
                                            new { Name = "start_offset" },
                                            new { Name = "end_offset" },
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        [TestMethod]
        public void TypedMessageDecoder_FileMetadata()
        {
            IReadOnlyList<TypedMessage> actual = ParseBinary("file-metadata.pb", "FileMetadataInfo");
            actual.Should().BeEquivalentTo(new[]
            {
                new
                {
                    Fields = new[]
                    {
                        new
                        {
                            Name = "file_path",
                            Value = new { Value = (object)@"C:\Projects\Sprints\UtilityAnalyzerPerf\Benchmark\Projects\fluentassertions\Tests\AssemblyB\ClassB.cs" }
                        },
                        new
                        {
                            Name = "encoding",
                            Value = new { Value = (object)@"utf-8" }
                        },
                    }
                },
                new
                {
                    Fields = new[]
                    {
                        new
                        {
                            Name = "file_path",
                            Value = new { Value = (object)@"C:\Projects\Sprints\UtilityAnalyzerPerf\Benchmark\Projects\fluentassertions\Tests\AssemblyB\ClassC.cs" }
                        },
                        new
                        {
                            Name = "encoding",
                            Value = new { Value = (object)@"utf-8" }
                        },
                    }
                },
                new
                {
                    Fields = new[]
                    {
                        new
                        {
                            Name = "file_path",
                            Value = new { Value = (object)@"C:\Projects\Sprints\UtilityAnalyzerPerf\Benchmark\Projects\fluentassertions\Tests\AssemblyB\obj\Debug\netstandard2.0\.NETStandard,Version=v2.0.AssemblyAttributes.cs" }
                        },
                        new
                        {
                            Name = "is_generated",
                            Value = new { Value = (object)true }
                        },
                        new
                        {
                            Name = "encoding",
                            Value = new { Value = (object)@"utf-8" }
                        },
                    }
                },
                new
                {
                    Fields = new[]
                    {
                        new
                        {
                            Name = "file_path",
                            Value = new { Value = (object)@"C:\Projects\Sprints\UtilityAnalyzerPerf\Benchmark\Projects\fluentassertions\Tests\AssemblyB\obj\Debug\netstandard2.0\AssemblyB.AssemblyInfo.cs" }
                        },
                        new
                        {
                            Name = "is_generated",
                            Value = new { Value = (object)true }
                        },
                        new
                        {
                            Name = "encoding",
                            Value = new { Value = (object)@"utf-8" }
                        },
                    }
                },
            });
        }

        [TestMethod]
        public void TypedMessageDecoder_Log()
        {
            IReadOnlyList<TypedMessage> actual = ParseBinary("log.pb", "LogInfo");
            actual.Should().BeEquivalentTo(new[]
            {
                new
                {
                    MessageType = "LogInfo",
                    Fields = new[]
                    {
                        new { Value = new { Value =  (object)2 } },
                        new { Value = new { Value =  (object)"Roslyn version: 4.8.0.0" } },
                    },
                },
                new
                {
                    MessageType = "LogInfo",
                    Fields = new[]
                    {
                        new { Value = new { Value =  (object)2 } },
                        new { Value = new { Value =  (object)"Language version: CSharp11" } },
                    },
                },
                new
                {
                    MessageType = "LogInfo",
                    Fields = new[]
                    {
                        new { Value = new { Value =  (object)2 } },
                        new { Value = new { Value =  (object)"Concurrent execution: enabled" } },
                    },
                },
                new
                {
                    MessageType = "LogInfo",
                    Fields = new[]
                    {
                        new { Value = new { Value =  (object)1 } },
                        new { Value = new { Value =  (object)@"File 'C:\Projects\Sprints\UtilityAnalyzerPerf\Benchmark\Projects\fluentassertions\Tests\AssemblyB\obj\Debug\netstandard2.0\.NETStandard,Version=v2.0.AssemblyAttributes.cs' was recognized as generated" } },
                    },
                },
                new
                {
                    MessageType = "LogInfo",
                    Fields = new[]
                    {
                        new { Value = new { Value =  (object)1 } },
                        new { Value = new { Value =  (object)@"File 'C:\Projects\Sprints\UtilityAnalyzerPerf\Benchmark\Projects\fluentassertions\Tests\AssemblyB\obj\Debug\netstandard2.0\AssemblyB.AssemblyInfo.cs' was recognized as generated" } },
                    },
                },
            });
        }

        [TestMethod]
        public void TypedMessageDecoder_Metrics()
        {
            IReadOnlyList<TypedMessage> actual = ParseBinary("metrics.pb", "MetricsInfo");
            actual.Should().BeEquivalentTo(new[]{
                new { MessageType = "MetricsInfo", },
                new { MessageType = "MetricsInfo", },
            });
        }


        private static IReadOnlyList<TypedMessage> ParseBinary(string pbFile, string protoDefinition)
        {
            var bytes = File.ReadAllBytes(pbFile);
            using var memory = new MemoryStream(bytes.Skip(0).ToArray());
            using var stream = CodedInputStream.CreateWithLimits(memory, int.MaxValue, int.MaxValue);
            var decoder = new TypedMessageDecoder();
            var proto = ProtoParser.ParseFile("AnalyzerReport.proto");
            var tokenTypeInfo = proto.topLevelDef().Where(x => x.messageDef()?.messageName().GetText() == protoDefinition).First().messageDef();
            var actual = decoder.Parse(stream, proto, tokenTypeInfo);
            return actual;
        }
    }
}