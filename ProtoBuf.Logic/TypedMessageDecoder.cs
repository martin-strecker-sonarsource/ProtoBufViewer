using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Google.Protobuf;
using static ProtoBuf.Antlr.Protobuf3Parser;

namespace ProtoBuf.Logic
{
    public abstract record class ProtoType(string Type, ParserRuleContext? TypeDefinition)
    {
        public string Definition => TypeDefinition?.GetFullText() ?? "No definition";
    }
    public sealed record class TypedDouble(double Value, ParserRuleContext? TypeDefinition) : ProtoType("Double", TypeDefinition);
    public sealed record class TypedFloat(float Value, ParserRuleContext? TypeDefinition) : ProtoType("Float", TypeDefinition);
    public sealed record class TypedInt32(Int32 Value, ParserRuleContext? TypeDefinition) : ProtoType("Int32", TypeDefinition);
    public sealed record class TypedInt64(Int64 Value, ParserRuleContext? TypeDefinition) : ProtoType("Int64", TypeDefinition);
    public sealed record class TypedUint32(UInt32 Value, ParserRuleContext? TypeDefinition) : ProtoType("UInt32", TypeDefinition);
    public sealed record class TypedUint64(UInt64 Value, ParserRuleContext? TypeDefinition) : ProtoType("UInt64", TypeDefinition);
    public sealed record class TypedSint32(Int32 Value, ParserRuleContext? TypeDefinition) : ProtoType("SInt32", TypeDefinition);
    public sealed record class TypedSint64(Int64 Value, ParserRuleContext? TypeDefinition) : ProtoType("SInt64", TypeDefinition);
    public sealed record class TypedFixed32(UInt32 Value, ParserRuleContext? TypeDefinition) : ProtoType("Fixed32", TypeDefinition);
    public sealed record class TypedFixed64(UInt64 Value, ParserRuleContext? TypeDefinition) : ProtoType("Fixed64", TypeDefinition);
    public sealed record class TypedSfixed32(Int32 Value, ParserRuleContext? TypeDefinition) : ProtoType("Sfixed32", TypeDefinition);
    public sealed record class TypedSfixed64(Int64 Value, ParserRuleContext? TypeDefinition) : ProtoType("Sfixed64", TypeDefinition);
    public sealed record class TypedBool(bool Value, ParserRuleContext? TypeDefinition) : ProtoType("Bool", TypeDefinition);
    public sealed record class TypedString(string Value, ParserRuleContext? TypeDefinition) : ProtoType("String", TypeDefinition);
    public sealed record class TypedBytes(ByteString Value, ParserRuleContext? TypeDefinition) : ProtoType("Bytes", TypeDefinition);
    public sealed record class TypedUnknown(object Value) : ProtoType("Unknown", null);
    public sealed record class TypedMessage(IReadOnlyList<TypedField> Fields, ParserRuleContext? TypeDefinition, MessageDefContext? MessageDef) : ProtoType("Message", TypeDefinition)
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
    public sealed record class TypedEnum(int Value, ParserRuleContext? TypeDefinition, EnumDefContext? EnumDef) : ProtoType("Enum", TypeDefinition)
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

    public sealed record class TypedField(string Name, int Index, ParserRuleContext? TypeDefinition, ProtoType Value): ProtoType(Value.Type, TypeDefinition);

    public class TypedMessageDecoder
    {
        public async Task<IReadOnlyList<TypedMessage>> Parse(CodedInputStream stream, Func<double, Task> progress, ProtoContext protoContext, MessageDefContext initialMessage)
        {
            var result = new List<TypedMessage>();
            var context = new BindingContext();
            while (!stream.IsAtEnd)
            {
                var message = await Task.Run(async () =>
                {
                    await progress((double)(stream.Position * 100) / stream.SizeLimit);
                    var binder = new MessageBinder(protoContext, initialMessage, context);
                    stream.ReadRawMessage(binder);
                    return binder.Result;
                });
                if (message != null)
                {
                    result.Add(message);
                }
            }
            return result;
        }
    }
}
