using Froto.Parser;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Microsoft.FSharp.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;
using static Google.Protobuf.WireFormat;

namespace ProtoBufViewer
{
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        private FSharpList<Ast.PStatement>? parseResult;
        private Ast.PStatement.TMessage? selectedMessage;

        public MainPageViewModel()
        {
            OpenProtoCommand = new Command(OpenProto);
            OpenBinaryCommand = new Command(OpenOpenBinary, _ => SelectedMessage != null);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Command OpenProtoCommand { get; }
        public Command OpenBinaryCommand { get; }

        public ObservableCollection<Ast.PStatement.TMessage> Messages { get; } = new();
        public Ast.PStatement.TMessage? SelectedMessage { get => selectedMessage; set { selectedMessage = value; OnPropertyChanged(); OpenBinaryCommand.ChangeCanExecute(); } }

        protected FSharpList<Ast.PStatement>? ParseResult { get => parseResult; set { parseResult = value; OnPropertyChanged(); } }

        private async void OpenProto(object o)
        {
            var pick = await FilePicker.Default.PickAsync(new PickOptions
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { "proto" } }
                })
            });
            if (pick is not null)
            {
                var pStatements = Parse.fromFile(pick.FullPath);
                var messages = pStatements.Where(s => s.IsTMessage).Cast<Ast.PStatement.TMessage>().ToList();
                Messages.Clear();
                foreach (var message in messages)
                {
                    Messages.Add(message);
                }
            }
        }

        private async void OpenOpenBinary(object o)
        {
            var pick = await FilePicker.Default.PickAsync(new PickOptions
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { "pb" } }
                })
            });
            if (pick is not null)
            {
                using var file = new FileStream(pick.FullPath, FileMode.Open, FileAccess.Read);
                using var coded = CodedInputStream.CreateWithLimits(file, int.MaxValue, int.MaxValue);
                var tag = coded.ReadWireTag(); // ignore the first Tag
                tag = coded.ReadWireTag();
                var text = coded.ReadString();
                tag = coded.PeekWireTag();
                var message = new TestMessage();
                coded.ReadRawMessage(message);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }

    class TestMessage : IMessage
    {
        public MessageDescriptor Descriptor => throw new NotImplementedException();

        public int CalculateSize() => throw new NotImplementedException();

        public void WriteTo(CodedOutputStream output) => throw new NotImplementedException();
        
        public void MergeFrom(CodedInputStream input)
        {
            var tag = input.ReadTag();
            var enuma = input.ReadEnum();
        }
    }
}
