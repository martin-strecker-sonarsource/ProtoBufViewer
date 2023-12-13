using Antlr4.Runtime;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using static Google.Protobuf.WireFormat;
using static ProtoBuf.Antlr.Protobuf3Parser;

namespace ProtoBuf.Logic
{
    internal class MessageBinder : IMessage
    {
        private readonly ProtoContext protoContext;
        private readonly MessageDefContext? messageDef;

        public MessageBinder(ProtoContext protoContext, MessageDefContext? messageDef)
        {
            this.protoContext = protoContext;
            this.messageDef = messageDef;
        }
        public TypedMessage? Result { get; internal set; }

        public void MergeFrom(CodedInputStream input)
        {
            var length = input.ReadLength();
            var targetPosition = input.Position + length;
            var fields = new List<TypedField>();
            while (input.Position < targetPosition && !input.IsAtEnd)
            {
                var (index, type) = input.ReadWireTag();
                var field = messageDef.messageBody().messageElement().Select(x => x.field()).Where(x => int.TryParse(x?.fieldNumber()?.GetText(), out var i) && i == index).FirstOrDefault();
                var parsedField = field != null && FitsFieldType(type, field.type_())
                    ? ParseField(input, protoContext, messageDef, field)
                    : ParseUnknownField(input, new WireTag(index, type));
                fields.Add(parsedField);
            }
            Result = new TypedMessage(fields, messageDef);
        }

        private TypedField ParseUnknownField(CodedInputStream stream, WireTag wireTag)
        {
            var (index, type) = wireTag;
            var value = stream.ReadType(type);
            return new TypedField("Unknown", index, new TypedUnknown(value));
        }

        private TypedField ParseField(CodedInputStream stream, ProtoContext protoContext, MessageDefContext message, FieldContext field)
        {
            var value = ReadExpectedType(stream, protoContext, field.type_());
            return new TypedField(field.fieldName().GetText(), int.Parse(field.fieldNumber().GetText()), value);
        }

        private static ProtoType? ReadExpectedType(CodedInputStream stream, ProtoContext protoContext, Type_Context expectedType)
        {
            if (expectedType.INT32() is not null)
                return new TypedInt32(stream.ReadInt32());
            else if (expectedType.INT64() is not null)
                return new TypedInt64(stream.ReadInt64());
            else if (expectedType.SINT32() is not null)
                return new TypedSint32(stream.ReadSInt32());
            else if (expectedType.SINT64() is not null)
                return new TypedSint64(stream.ReadSInt64());
            else if (expectedType.UINT32() is not null)
                return new TypedUint32(stream.ReadUInt32());
            else if (expectedType.UINT64() is not null)
                return new TypedUint64(stream.ReadUInt64());
            else if (expectedType.BOOL() is not null)
                return new TypedBool(stream.ReadBool());
            else if (expectedType.FIXED32() is not null)
                return new TypedFixed32(stream.ReadFixed32());
            else if (expectedType.FIXED64() is not null)
                return new TypedFixed64(stream.ReadFixed64());
            else if (expectedType.SFIXED32() is not null)
                return new TypedSfixed32(stream.ReadSFixed32());
            else if (expectedType.SFIXED64() is not null)
                return new TypedSfixed64(stream.ReadSFixed64());
            else if (expectedType.DOUBLE() is not null)
                return new TypedDouble(stream.ReadDouble());
            else if (expectedType.FLOAT() is not null)
                return new TypedFloat(stream.ReadFloat());
            else if (expectedType.STRING() is not null)
                return new TypedString(stream.ReadString());
            else if (expectedType.enumType() is not null || expectedType.messageType() is not null)
            {
                var (enumType, messageType) = (expectedType.enumType(), expectedType.messageType());
                var bound = BindMessageOrEnumDef(protoContext, DottedNames(enumType).Concat(DottedNames(messageType)), expectedType);
                return bound switch
                {
                    EnumDefContext enumDef => new TypedEnum(stream.ReadEnum(), enumDef),
                    MessageDefContext innerMessageDef => ParseMessage(stream, protoContext, innerMessageDef),
                    _ => throw new NotImplementedException(),
                };
            }
            return null;
        }

        static IEnumerable<string> DottedNames(MessageTypeContext message)
        {
            if (message == null)
            {
                yield break;
            }
            var (dot, ident, name) = (message.DOT(), message.ident(), message.messageName());
            if (dot != null && dot.FirstOrDefault() is { Symbol.Text: { } dotText })
            {
                yield return dotText;
            }
            foreach (var item in ident)
            {
                yield return item.GetText();
            }
            yield return name.GetText();
        }

        static IEnumerable<string> DottedNames(EnumTypeContext message)
        {
            if (message == null)
            {
                yield break;
            }

            var (dot, ident, name) = (message.DOT(), message.ident(), message.enumName());
            if (dot != null && dot.FirstOrDefault() is { Symbol.Text: { } dotText })
            {
                yield return dotText;
            }
            foreach (var item in ident)
            {
                yield return item.GetText();
            }
            yield return name.GetText();
        }

        private static ParserRuleContext? BindMessageOrEnumDef(ProtoContext protoContext, IEnumerable<string> names, Type_Context expectedType) =>
            BindMessageDef(protoContext, names, expectedType) ?? BindEnumDef(protoContext, names, expectedType);

        private static ParserRuleContext? BindMessageDef(ProtoContext protoContext, IEnumerable<string> names, Type_Context expectedType)
        {
            bool rootSearch = names.FirstOrDefault() == ".";
            var searchRoot = rootSearch
                ? protoContext.topLevelDef().Select(x => x.messageDef())
                : expectedType.AncestorsAndSelf().OfType<MessageDefContext>().SelectMany(x => x.messageBody().messageElement().Select(x => x.messageDef()));
            searchRoot = searchRoot.Where(x => x != null);
            var result = names.Skip(rootSearch ? 1 : 0).Aggregate(searchRoot, (parent, name) => [parent.FirstOrDefault(x => x?.messageName()?.GetText() == name)], x => x?.FirstOrDefault());
            if (result is null && !rootSearch)
            {
                return BindMessageDef(protoContext, new[] { "." }.Concat(names), expectedType);
            }
            return result;
        }

        private static ParserRuleContext? BindEnumDef(ProtoContext protoContext, IEnumerable<string> names, Type_Context expectedType)
        {
            bool rootSearch = names.FirstOrDefault() == ".";
            var searchRoot = rootSearch
                ? protoContext.topLevelDef().Select(x => x.enumDef()).FirstOrDefault(x => x != null)
                : expectedType.AncestorsAndSelf().OfType<EnumDefContext>().FirstOrDefault();
            var result = names.Skip(rootSearch ? 1 : 0).Aggregate(searchRoot, (parent, name) => parent?.enumName().GetText() == name ? parent : null);
            if (result == null && !rootSearch)
            {
                return BindEnumDef(protoContext, new[] { "." }.Concat(names), expectedType);
            }
            return result;
        }

        private static ProtoType? ParseMessage(CodedInputStream stream, ProtoContext protoContext, MessageDefContext? messageDef)
        {
            var builder = new MessageBinder(protoContext, messageDef);
            stream.ReadRawMessage(builder);
            return builder.Result;
        }

        private bool FitsFieldType(WireType actual, Type_Context expected)
        {
            return actual switch
            {
                WireType.Varint => expected.INT32() is not null || expected.INT64() is not null || expected.UINT32() is not null || expected.SINT32() is not null || expected.SINT64() is not null || expected.BOOL() is not null || expected.enumType() is not null || expected.messageType() is not null,
                WireType.Fixed64 => expected.FIXED64() is not null || expected.SFIXED64() is not null || expected.DOUBLE() is not null,
                WireType.LengthDelimited => expected.STRING() is not null || expected.BYTES() is not null || expected.messageType() is not null,
                WireType.StartGroup => throw new NotSupportedException(),
                WireType.EndGroup => throw new NotSupportedException(),
                WireType.Fixed32 => expected.FIXED32() is not null || expected.SFIXED32() is not null || expected.FLOAT() is not null,
                _ => throw new NotSupportedException(),
            };
        }

        MessageDescriptor IMessage.Descriptor => throw new NotImplementedException();
        void IMessage.WriteTo(CodedOutputStream output) => throw new NotImplementedException();
        int IMessage.CalculateSize() => throw new NotImplementedException();
    }
}
