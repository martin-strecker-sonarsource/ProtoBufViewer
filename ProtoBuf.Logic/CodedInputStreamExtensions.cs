using Google.Protobuf;
using static Google.Protobuf.WireFormat;

namespace ProtoBuf.Logic
{
    public readonly record struct WireTag(int FieldNumber, WireType WireType);

    public static class CodedInputStreamExtensions
    {
        public static WireTag ReadWireTag(this CodedInputStream stream) =>
            ConvertToWireTag(stream.ReadTag());


        public static WireTag PeekWireTag(this CodedInputStream stream) =>
            ConvertToWireTag(stream.PeekTag());

        public static object ReadType(this CodedInputStream stream, WireType wireType) =>
            wireType switch
            {
                WireType.Varint => stream.ReadInt64(),
                WireType.Fixed64 => stream.ReadFixed64(),
                WireType.LengthDelimited => stream.ReadString(),
                WireType.StartGroup => throw new NotSupportedException(),
                WireType.EndGroup => throw new NotSupportedException(),
                WireType.Fixed32 => stream.ReadFixed32(),
                _ => throw new NotSupportedException(),
            };

        private static WireTag ConvertToWireTag(uint tag) =>
            new(GetTagFieldNumber(tag), GetTagWireType(tag));
    }
}
