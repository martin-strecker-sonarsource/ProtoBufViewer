using System.Diagnostics.CodeAnalysis;

namespace ProtoBufViewer
{
    public record class MessageViewModel(Protobuf3Parser.MessageDefContext MessageDefContext, string Name, MessageViewModel? Parent)
    {
        private readonly List<MessageViewModel> childs = new();

        public IReadOnlyCollection<MessageViewModel> Childs { get => childs; }

        internal class Listener : Protobuf3BaseListener
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
                    parent.childs.Add(message);
                }
                _Parents.Push(message);
                base.EnterMessageDef(context);
            }

            public override void ExitMessageDef([NotNull] Protobuf3Parser.MessageDefContext context)
            {
                _Parents.Pop();
                base.ExitMessageDef(context);
            }
        }
    }
}
