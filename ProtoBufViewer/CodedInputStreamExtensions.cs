using Google.Protobuf;
using static Google.Protobuf.WireFormat;

namespace ProtoBufViewer
{
    public readonly record struct WireTag(uint FieldNumber, WireType WireType);

    public static class CodedInputStreamExtensions
    {
        public static WireTag ReadWireTag(this CodedInputStream stream) =>
            ConvertToWireTag(stream.ReadTag());


        public static WireTag PeekWireTag(this CodedInputStream stream) =>
            ConvertToWireTag(stream.PeekTag());

        private static WireTag ConvertToWireTag(uint tag) =>
            new(tag >> 3, (WireType)(tag & 0x07));
    }
}
