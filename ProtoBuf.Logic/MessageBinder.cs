using Antlr4.Runtime;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using ProtoBuf.Antlr;
using System.Collections.Concurrent;
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

        public MessageBinder(ProtoContext protoContext, MessageDefContext? messageDef) : this(protoContext, messageDef, null) { }

        private MessageBinder(ProtoContext protoContext, MessageDefContext? messageDef, ConcurrentDictionary<(Type_Context, string names), ParserRuleContext?>? cache)
        {
            ProtoContext = protoContext;
            MessageDef = messageDef;
            BindingCache = cache ?? new();
        }

        private ConcurrentDictionary<(Type_Context, string names), ParserRuleContext?> BindingCache { get; }
        public ProtoContext ProtoContext { get; }
        public MessageDefContext? MessageDef { get; }
        public TypedMessage? Result { get; private set; }

        public void MergeFrom(CodedInputStream input)
        {
            var length = input.ReadLength();
            var targetPosition = input.Position + length;
            var fields = new List<TypedField>();
            while (input.Position < targetPosition && !input.IsAtEnd)
            {
                var (index, type) = input.ReadWireTag();
                var field = MessageDef?.messageBody().messageElement().Select(x => x.field()).FirstOrDefault(x => int.TryParse(x?.fieldNumber()?.GetText(), out var i) && i == index);
                var parsedFields = MessageDef != null && field != null && FitsFieldType(type, field.type_()) is var fieldType and not FieldType.Unknown
                    ? ParseField(input, fieldType, field)
                    : ParseUnknownField(input, new WireTag(index, type));
                fields.AddRange(parsedFields);
            }
            Result = new TypedMessage(fields, MessageDef, MessageDef);
        }

        private IEnumerable<TypedField> ParseUnknownField(CodedInputStream stream, WireTag wireTag)
        {
            var (index, type) = wireTag;
            var value = stream.ReadType(type);
            yield return new TypedField("Unknown", index, null, new TypedUnknown(value));
        }

        private IEnumerable<TypedField> ParseField(CodedInputStream stream, FieldType type, FieldContext field)
        {
            var (fieldName, index) = (field.fieldName().GetText(), int.Parse(field.fieldNumber().GetText()));
            var values = ReadExpectedType(stream, type, field.type_());
            foreach (var value in values)
            {
                yield return new TypedField(fieldName, index, field, value);
            }
        }

        private IEnumerable<ProtoType> ReadExpectedType(CodedInputStream stream, FieldType type, Type_Context expectedType)
        {
            if (type == FieldType.Packed)
            {
                var length = stream.ReadLength();
                var expectedEnd = stream.Position + length;
                while (stream.Position < expectedEnd)
                {
                    if (ReadExpectedType(stream, expectedType) is { } protoType)
                    {
                        yield return protoType;
                    }
                }
            }
            else
            {
                if (ReadExpectedType(stream, expectedType) is { } protoType)
                {
                    yield return protoType;
                }
            }
        }

        private ProtoType? ReadExpectedType(CodedInputStream stream, Type_Context expectedType)
        {
            if (expectedType.INT32() is not null)
                return new TypedInt32(stream.ReadInt32(), expectedType);
            else if (expectedType.INT64() is not null)
                return new TypedInt64(stream.ReadInt64(), expectedType);
            else if (expectedType.SINT32() is not null)
                return new TypedSint32(stream.ReadSInt32(), expectedType);
            else if (expectedType.SINT64() is not null)
                return new TypedSint64(stream.ReadSInt64(), expectedType);
            else if (expectedType.UINT32() is not null)
                return new TypedUint32(stream.ReadUInt32(), expectedType);
            else if (expectedType.UINT64() is not null)
                return new TypedUint64(stream.ReadUInt64(), expectedType);
            else if (expectedType.BOOL() is not null)
                return new TypedBool(stream.ReadBool(), expectedType);
            else if (expectedType.FIXED32() is not null)
                return new TypedFixed32(stream.ReadFixed32(), expectedType);
            else if (expectedType.FIXED64() is not null)
                return new TypedFixed64(stream.ReadFixed64(), expectedType);
            else if (expectedType.SFIXED32() is not null)
                return new TypedSfixed32(stream.ReadSFixed32(), expectedType);
            else if (expectedType.SFIXED64() is not null)
                return new TypedSfixed64(stream.ReadSFixed64(), expectedType);
            else if (expectedType.DOUBLE() is not null)
                return new TypedDouble(stream.ReadDouble(), expectedType);
            else if (expectedType.FLOAT() is not null)
                return new TypedFloat(stream.ReadFloat(), expectedType);
            else if (expectedType.STRING() is not null)
                return new TypedString(stream.ReadString(), expectedType);
            else if (expectedType.enumType() is not null || expectedType.messageType() is not null)
            {
                return BindMessageOrEnumDef(expectedType) switch
                {
                    EnumDefContext enumDef => new TypedEnum(stream.ReadEnum(), expectedType, enumDef),
                    MessageDefContext innerMessageDef => ParseMessage(stream, innerMessageDef),
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

        private ParserRuleContext? BindMessageOrEnumDef(Type_Context expectedType)
        {
            var names = (expectedType.enumType(), expectedType.messageType()) switch
            {
                (null, { } messageType) => DottedNames(messageType),
                ({ } enumType, null) => DottedNames(enumType),
                _ => throw new InvalidOperationException(),
            };
            return BindCached(expectedType, names)
                ?? BindType(names, expectedType, names => new MessageDefBinder(names)) as ParserRuleContext
                ?? BindType(names, expectedType, names => new EnumDefBinder(names));
        }

        private ParserRuleContext? BindCached(Type_Context expectedType, IEnumerable<string> names)
        {
            var namesString = string.Join(".", names);
            var key = (expectedType, namesString);
            if (BindingCache.TryGetValue(key, out var cached))
            {
                return cached;
            }
            else
            {
                var result = BindType(names, expectedType, names => new MessageDefBinder(names)) as ParserRuleContext
                    ?? BindType(names, expectedType, names => new EnumDefBinder(names));
                BindingCache.TryAdd(key, result);
                return result;
            }
        }

        private T? BindType<T>(IEnumerable<string> names, Type_Context expectedType, Func<IEnumerable<string>, IProtobuf3Visitor<T>> visitorFactory)
        {
            var rootSearch = names.FirstOrDefault() == ".";
            if (!rootSearch
                && expectedType.AncestorsAndSelf().OfType<MessageDefContext>().FirstOrDefault() is { } parentDefinitionContext
                && visitorFactory(names).Visit(parentDefinitionContext.messageBody()) is { } relative)
            {
                return relative;
            }
            return visitorFactory(rootSearch ? names.Skip(1) : names).Visit(ProtoContext);
        }

        private ProtoType? ParseMessage(CodedInputStream stream, MessageDefContext? messageDef)
        {
            var builder = new MessageBinder(ProtoContext, messageDef, BindingCache);
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

        sealed class MessageDefBinder(IEnumerable<string> names) : Protobuf3BaseVisitor<MessageDefContext>
        {
            private readonly Queue<string> Names = new(names);
            protected override MessageDefContext? AggregateResult(MessageDefContext aggregate, MessageDefContext nextResult) =>
                aggregate ?? nextResult;

            public override MessageDefContext? VisitMessageDef(MessageDefContext context)
            {
                if (Names.TryPeek(out var name) && context.messageName().GetText() == name)
                {
                    Names.Dequeue();
                    if (Names.Count == 0)
                    {
                        return context;
                    }
                    return base.VisitMessageDef(context);
                }
                return null;
            }
        }

        sealed class EnumDefBinder(IEnumerable<string> names) : Protobuf3BaseVisitor<EnumDefContext>
        {
            private readonly Queue<string> Names = new(names);
            protected override EnumDefContext? AggregateResult(EnumDefContext? aggregate, EnumDefContext? nextResult) =>
                aggregate ?? nextResult;

            public override EnumDefContext? VisitEnumDef(EnumDefContext context)
            {
                if (Names.TryPeek(out var name) && context.enumName().GetText() == name)
                {
                    Names.Dequeue();
                    if (Names.Count == 0)
                    {
                        return context;
                    }
                    return base.VisitEnumDef(context);
                }
                return null;
            }
        }
    }
}
