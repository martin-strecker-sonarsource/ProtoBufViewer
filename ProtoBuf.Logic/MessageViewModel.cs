using Antlr4.Runtime.Misc;
using Antlr4.Runtime;
using ProtoBuf.Antlr;
using System.Diagnostics;

namespace ProtoBuf.Logic
{
    public record class MessageViewModel(Protobuf3Parser.MessageDefContext MessageDefContext, string Name, MessageViewModel? Parent)
    {
        private readonly List<MessageViewModel> nested = new();
        private readonly List<FieldViewModel> fields = new();
        public IReadOnlyCollection<MessageViewModel> Nested { get => nested; }
        public IReadOnlyCollection<FieldViewModel> Fields { get => fields; }

        public string Definition => GetFullText(MessageDefContext);

        public static string GetFullText(ParserRuleContext context)
        {
            if (context.Start == null || context.Stop == null || context.Start.StartIndex < 0 || context.Stop.StopIndex < 0)
                return context.GetText(); // Fallback

            return context.Start.InputStream.GetText(Interval.Of(context.Start.StartIndex, context.Stop.StopIndex));
        }

        public class Visitor : Protobuf3BaseVisitor<bool>
        {
            private MessageViewModel? parent = null;
            private readonly List<MessageViewModel> messages = new List<MessageViewModel>();

            public IReadOnlyCollection<MessageViewModel> Messages => messages;

            public override bool VisitMessageDef(Protobuf3Parser.MessageDefContext context)
            {
                var message = new MessageViewModel(context, context.messageName().GetText(), parent);
                messages.Add(message);
                (var oldParent, parent) = (parent, message);
                var result = base.VisitMessageDef(context);
                parent = oldParent;
                return result;
            }

            public override bool VisitField(Protobuf3Parser.FieldContext context)
            {
                string name = context.fieldName().GetText();
                int index = int.Parse(context.fieldNumber().intLit().GetText());
                Debug.Assert(parent != null);
                parent.fields.Add(new FieldViewModel(context, parent, name, index));
                return base.VisitField(context);
            }
        }
    }

    public record class FieldViewModel(Protobuf3Parser.FieldContext FieldContext, MessageViewModel Message, string Name, int Index);
}
