using Google.Protobuf;
using Microsoft.AspNetCore.Components.Forms;
using ProtoBuf.Logic;
using static ProtoBuf.Antlr.Protobuf3Parser;

namespace ProtoBufViewer.Blazor.Pages
{
    public partial class Index
    {
        private const string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full z-10";
        private IBrowserFile? ProtoFile;
        private IBrowserFile? ProtoBinFile;
        HashSet<MessageViewModel> Messages { get; } = new();
        MessageViewModel? SelectedMessage { get; set; }
        ProtoContext? ParseResult { get; set; }

        List<ProtoType>? TypedMessages { get; set; }

        private async Task MessageFileChanged(IBrowserFile file)
        {
            ProtoFile = file;
            Messages.Clear();
            using var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms);
            ms.Position = 0;
            ParseResult = ProtoParser.ParseFile(ms);
            var visitor = new MessageViewModel.Visitor();
            visitor.Visit(ParseResult);
            foreach (var m in visitor.Messages.Where(x => x.Parent == null))
            {
                Messages.Add(m);
            }
        }

        private async Task BinFilesChanged(IBrowserFile file)
        {
            ProtoBinFile = file;
            using var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms);
            ms.Position = 0;
            using var coded = CodedInputStream.CreateWithLimits(ms, int.MaxValue, int.MaxValue);
            var decoder = new TypedMessageDecoder();
            var result = decoder.Parse(coded, ParseResult, SelectedMessage.MessageDefContext);
            TypedMessages = new(result);
        }
    }
}
