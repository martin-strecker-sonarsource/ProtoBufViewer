using ProtoBuf.Antlr;
using System.Diagnostics.CodeAnalysis;

namespace ProtoBuf.Logic
{
    public record class MessageViewModel(Protobuf3Parser.MessageDefContext MessageDefContext, string Name, MessageViewModel? Parent)
    {
        private readonly List<MessageViewModel> nested = new();
        private readonly List<FieldViewModel> fields = new();
        public IReadOnlyCollection<MessageViewModel> Nested { get => nested; }
        public IReadOnlyCollection<FieldViewModel> Fields { get => fields; }

        public class Listener : Protobuf3BaseListener
        {
            readonly List<MessageViewModel> _Messages = [];
            readonly Stack<MessageViewModel> _Parents = new();
            public IReadOnlyList<MessageViewModel> Messages { get => _Messages; }

            public override void EnterMessageDef([NotNull] Protobuf3Parser.MessageDefContext context)
            {
                _Parents.TryPeek(out var parent);
                var message = new MessageViewModel(context, context.messageName().GetText(), parent);
                if (parent == null)
                {
                    _Messages.Add(message);
                }
                else
                {
                    parent.nested.Add(message);
                }
                _Parents.Push(message);
            }

            public override void ExitMessageDef([NotNull] Protobuf3Parser.MessageDefContext context) =>
                _Parents.Pop();

            public override void EnterField([Antlr4.Runtime.Misc.NotNull] Protobuf3Parser.FieldContext context)
            {
                var parent = _Parents.Peek();
                parent.fields.Add(new FieldViewModel(context, parent, context.fieldName().GetText(), int.Parse(context.fieldNumber().intLit().GetText())));
            }
        }
    }
    
    public record class FieldViewModel(Protobuf3Parser.FieldContext FieldContext, MessageViewModel Message, string Name, int Index);
}
