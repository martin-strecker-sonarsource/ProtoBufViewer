using Google.Protobuf;
using ProtoBuf.Logic;

namespace ProtoBufTests
{
    [TestClass]
    [DeploymentItem(@"Artefacts\AnalyzerReport.proto")]
    [DeploymentItem(@"Artefacts\token-type.pb")]
    public class TypedMessageDecoderTests
    {
        [TestMethod]
        public void TypedMessageDecoder()
        {
            var proto = ProtoParser.ParseFile("AnalyzerReport.proto");
            var tokenTypeInfo = proto.topLevelDef().Where(x => x.messageDef()?.messageName().GetText() == "TokenTypeInfo").First().messageDef();
            var bytes = File.ReadAllBytes("token-type.pb");
            using var memory = new MemoryStream(bytes.Skip(0).ToArray());
            using var stream = CodedInputStream.CreateWithLimits(memory, int.MaxValue, int.MaxValue);
            var decoder = new TypedMessageDecoder();
            var actual = decoder.Parse(stream, proto, tokenTypeInfo);
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