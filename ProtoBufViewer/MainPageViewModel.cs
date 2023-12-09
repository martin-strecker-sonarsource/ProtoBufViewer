using Froto.Parser;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Microsoft.FSharp.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
                using var stream = new FileStream(pick.FullPath, FileMode.Open, FileAccess.Read);
                using var coded = Google.Protobuf.CodedInputStream.CreateWithLimits(stream, int.MaxValue, int.MaxValue);
                var tag = coded.PeekTag();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
