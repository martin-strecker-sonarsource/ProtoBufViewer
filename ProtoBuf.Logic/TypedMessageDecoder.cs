using Antlr4.Runtime.Misc;
using Google.Protobuf;
using static ProtoBuf.Antlr.Protobuf3Parser;

namespace ProtoBuf.Logic
{
    public abstract record class ProtoType(string Type);
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
    public sealed record class TypedMessage(IReadOnlyList<TypedField> Fields, MessageDefContext? MessageDef) : ProtoType("Message")
    {
        public string MessageType => MessageDef == null ? "Unknown message type" : MessageDef.messageName().GetText();

        public string Definition => GetFullText(MessageDef);

        public static string GetFullText(MessageDefContext context)
        {
            if (context.Start == null || context.Stop == null || context.Start.StartIndex < 0 || context.Stop.StopIndex < 0)
                return context.GetText(); // Fallback

            return context.Start.InputStream.GetText(Interval.Of(context.Start.StartIndex, context.Stop.StopIndex));
        }
    }
    public sealed record class TypedEnum(int Value, EnumDefContext? EnumDef) : ProtoType("Enum")
    {
        public string EnumType => EnumDef == null ? "Unknown enum type" : EnumDef.enumName().GetText();
        public string EnumValue => EnumDef?.enumBody().enumElement().FirstOrDefault(
            x => int.Parse(x.enumField().intLit().INT_LIT().Symbol.Text) == Value)?.enumField().ident().GetText() ?? "Unknown";
        public string Definition => GetFullText(EnumDef);

        public static string GetFullText(EnumDefContext context)
        {
            if (context.Start == null || context.Stop == null || context.Start.StartIndex < 0 || context.Stop.StopIndex < 0)
                return context.GetText(); // Fallback

            return context.Start.InputStream.GetText(Interval.Of(context.Start.StartIndex, context.Stop.StopIndex));
        }
    }

    public sealed record class TypedField(string Name, int Index, ProtoType Value);

    public class TypedMessageDecoder
    {
        public IReadOnlyList<TypedMessage> Parse(CodedInputStream stream, ProtoContext protoContext, MessageDefContext initialMessage)
        {
            var result = new List<TypedMessage>();
            while (!stream.IsAtEnd)
            {
                var binder = new MessageBinder(protoContext, initialMessage);
                stream.ReadRawMessage(binder);
                if (binder.Result != null)
                {
                    result.Add(binder.Result);
                }
            }
            return result;
        }
    }
}
