using FluentAssertions;
using Google.Protobuf;
using ProtoBuf.Logic;

namespace ProtoBufTests
{
    [TestClass]
    [DeploymentItem(@"Artefacts\AnalyzerReport.proto")]
    [DeploymentItem(@"Artefacts\token-type.pb")]
    [DeploymentItem(@"Artefacts\symrefs.pb.pb")]
    public class TypedMessageDecoderTests
    {
        [TestMethod]
        public void TypedMessageDecoder_TokenType()
        {
            var proto = ProtoParser.ParseFile("AnalyzerReport.proto");
            var tokenTypeInfo = proto.topLevelDef().Where(x => x.messageDef()?.messageName().GetText() == "TokenTypeInfo").First().messageDef();
            var bytes = File.ReadAllBytes("token-type.pb");
            using var memory = new MemoryStream(bytes.Skip(0).ToArray());
            using var stream = CodedInputStream.CreateWithLimits(memory, int.MaxValue, int.MaxValue);
            var decoder = new TypedMessageDecoder();
            var actual = decoder.Parse(stream, proto, tokenTypeInfo);
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
        public void Sample_Base64()
        {
            var bytes = File.ReadAllBytes("token-type.pb");
            var skipped = bytes.Skip(2).ToArray();
            var base64 = Convert.ToBase64String(skipped);
            // https://protobuf-decoder.netlify.app/
        }
    }
}