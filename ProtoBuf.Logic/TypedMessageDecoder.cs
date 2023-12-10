using Google.Protobuf;
using static Google.Protobuf.WireFormat;
using static Protobuf3Parser;

namespace ProtoBuf.Logic
{
    public abstract record class ProtoType(string type);
    public sealed record class TypedDouble(double Value) : ProtoType("Double");
    public sealed record class TypedFloat(float Value) : ProtoType("Float");
    public sealed record class TypedInt32(Int32 Value) : ProtoType("Int32");
    public sealed record class TypedInt64(Int64 Value) : ProtoType("Int64");
    public sealed record class TypedUint32(UInt32 Value) : ProtoType("UInt32");
    public sealed record class TypedUint64(UInt64 Value) : ProtoType("UInt64");
    public sealed record class TypedSint32(Int32 Value) : ProtoType("SInt32");
    public sealed record class TypedSint64(Int64 Value) : ProtoType("SInt64");
    public sealed record class TypedFixed32(UInt32 Value) : ProtoType("Fixed32");
    public sealed record class TypedFixed64(UInt64 Value) : ProtoType("Fixed64");
    public sealed record class TypedSfixed32(Int32 Value) : ProtoType("Sfixed32");
    public sealed record class TypedSfixed64(Int64 Value) : ProtoType("Sfixed64");
    public sealed record class TypedBool(bool Value) : ProtoType("Bool");
    public sealed record class TypedString(string Value) : ProtoType("String");
    public sealed record class TypedBytes(ByteString Value) : ProtoType("Bytes");
    public sealed record class TypedUnknown(object Value) : ProtoType("Unknown");
    public sealed record class TypedMessage(IDictionary<TypedField, ProtoType> Fields) : ProtoType("Message");
    public sealed record class TypedEnum(int value, EnumDefContext? EnumDef) : ProtoType("Enum");
    public sealed record class TypedField(string Name, int Index, ProtoType Value);

    public class TypedMessageDecoder
    {
        public TypedMessage? Parse(CodedInputStream stream, ProtoContext protoContext, MessageDefContext initialMessage)
        {
            while (!stream.IsAtEnd)
            {
                var binder = new MessageBinder(protoContext, initialMessage, int.MaxValue);
                stream.ReadMessage(binder);
            }
            return null;
        }
    }
}
