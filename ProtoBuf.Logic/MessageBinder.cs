using Antlr4.Runtime;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using static Google.Protobuf.WireFormat;
using static ProtoBuf.Antlr.Protobuf3Parser;

namespace ProtoBuf.Logic
{
    internal class MessageBinder : IMessage
    {
        enum FieldType
        {
            Unknown,
            Expected,
            Packed
        }

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
                var field = messageDef?.messageBody().messageElement().Select(x => x.field()).Where(x => int.TryParse(x?.fieldNumber()?.GetText(), out var i) && i == index).FirstOrDefault();
                var fieldType = FitsFieldType(type, field.type_());
                var parsedFields = messageDef != null && field != null && fieldType != FieldType.Unknown
                    ? ParseField(input, protoContext, fieldType, field)
                    : ParseUnknownField(input, new WireTag(index, type));
                fields.AddRange(parsedFields);
            }
            Result = new TypedMessage(fields, messageDef);
        }

        private IEnumerable<TypedField> ParseUnknownField(CodedInputStream stream, WireTag wireTag)
        {
            var (index, type) = wireTag;
            var value = stream.ReadType(type);
            yield return new TypedField("Unknown", index, new TypedUnknown(value));
        }

        private IEnumerable<TypedField> ParseField(CodedInputStream stream, ProtoContext protoContext, FieldType type, FieldContext field)
        {
            var (fieldName, index) = (field.fieldName().GetText(), int.Parse(field.fieldNumber().GetText()));
            var values = ReadExpectedType(stream, protoContext, type, field.type_());
            foreach (var value in values)
            {
                yield return new TypedField(fieldName, index, value);
            }
        }

        private static IEnumerable<ProtoType> ReadExpectedType(CodedInputStream stream, ProtoContext protoContext, FieldType type, Type_Context expectedType)
        {
            if (type == FieldType.Packed)
            {
                var length = stream.ReadLength();
                var expectedEnd = stream.Position + length;
                while (stream.Position < expectedEnd)
                {
                    yield return ReadExpectedType(stream, protoContext, expectedType);
                }
            }
            else
            {
                yield return ReadExpectedType(stream, protoContext, expectedType);
            }
        }

        private static ProtoType ReadExpectedType(CodedInputStream stream, ProtoContext protoContext, Type_Context expectedType)
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
                ? protoContext.topLevelDef().Select(x => x.enumDef())
                : expectedType.AncestorsAndSelf().OfType<EnumDefContext>();
            searchRoot = searchRoot.Where(x => x != null);
            var result = names.Skip(rootSearch ? 1 : 0).Aggregate(searchRoot, (parent, name) => [parent.FirstOrDefault(x => x.enumName().GetText() == name)], x => x?.FirstOrDefault());

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

        private FieldType FitsFieldType(WireType actual, Type_Context expected)
        {
            return actual switch
            {
                WireType.Varint => expected.INT32() is not null || expected.INT64() is not null || expected.UINT32() is not null || expected.SINT32() is not null || expected.SINT64() is not null || expected.BOOL() is not null || expected.enumType() is not null || expected.messageType() is not null ? FieldType.Expected : FieldType.Unknown,
                WireType.Fixed64 => expected.FIXED64() is not null || expected.SFIXED64() is not null || expected.DOUBLE() is not null ? FieldType.Expected : FieldType.Unknown,
                WireType.LengthDelimited when expected.STRING() is not null || expected.BYTES() is not null || expected.messageType() is not null => FieldType.Expected,
                WireType.LengthDelimited when expected.INT32() is not null || expected.INT64() is not null || expected.UINT32() is not null || expected.SINT32() is not null || expected.UINT64() is not null => FieldType.Packed,
                WireType.StartGroup => throw new NotSupportedException(),
                WireType.EndGroup => throw new NotSupportedException(),
                WireType.Fixed32 => expected.FIXED32() is not null || expected.SFIXED32() is not null || expected.FLOAT() is not null ? FieldType.Expected : FieldType.Unknown,
                _ => throw new NotSupportedException(),
            };
        }

        MessageDescriptor IMessage.Descriptor => throw new NotImplementedException();
        void IMessage.WriteTo(CodedOutputStream output) => throw new NotImplementedException();
        int IMessage.CalculateSize() => throw new NotImplementedException();
    }
}
