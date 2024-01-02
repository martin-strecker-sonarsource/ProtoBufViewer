using Google.Protobuf;
using Microsoft.AspNetCore.Components.Forms;
using ProtoBuf.Logic;
using System.Diagnostics;
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
        public double? Progress { get; set; }

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
            if (ParseResult == null || SelectedMessage == null) return;
            ProtoBinFile = file;
            using var ms = new MemoryStream(capacity: (int)file.Size);
            Progress = 0;
            await file.OpenReadStream(maxAllowedSize: file.Size).CopyToAsync(ms);
            ms.Position = 0;
            using var coded = CodedInputStream.CreateWithLimits(ms, (int)ms.Length, int.MaxValue);
            var decoder = new TypedMessageDecoder();
            var lastRender = Stopwatch.StartNew();
            var result = await decoder.Parse(coded, async progress =>
            {
                Progress = progress;
                if (lastRender.ElapsedMilliseconds > 1000)
                {
                    lastRender = Stopwatch.StartNew();
                    this.StateHasChanged();
                    await Task.Delay(50);
                }
            }, ParseResult, SelectedMessage.MessageDefContext);
            TypedMessages = new(result);
            Progress = null;
        }
    }
}
