using Antlr4.Runtime.Tree;
using CommunityToolkit.Mvvm.Input;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Microsoft.Win32;
using ProtoBuf.Logic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using static Protobuf3Parser;

namespace ProtoBufViewer.WPF;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private ProtoContext? parseResult;
    private MessageViewModel? selectedMessage;

    public MainWindowViewModel()
    {
        OpenProtoCommand = new AsyncRelayCommand(OpenProto);
        OpenBinaryCommand = new AsyncRelayCommand(OpenOpenBinary, () => SelectedMessage != null);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public AsyncRelayCommand OpenProtoCommand { get; }
    public AsyncRelayCommand OpenBinaryCommand { get; }

    public ObservableCollection<MessageViewModel> Messages { get; } = new();
    public MessageViewModel? SelectedMessage { get => selectedMessage; set { selectedMessage = value; OnPropertyChanged(); OpenBinaryCommand.NotifyCanExecuteChanged(); } }

    protected ProtoContext? ParseResult { get => parseResult; set { parseResult = value; OnPropertyChanged(); } }

    private async Task OpenProto()
    {
        OpenFileDialog openFileDialog = new();
        openFileDialog.Filter = "ProtoBuf Files (*.proto)|*.proto";
        if (openFileDialog.ShowDialog() is true)
        {
            var proto = ProtoParser.ParseFile(openFileDialog.FileName);
            ParseResult = proto;
            var walker = new ParseTreeWalker();
            var listener = new MessageViewModel.Listener();
            walker.Walk(listener, proto);
            Messages.Clear();
            foreach (var m in listener.Messages)
            {
                Messages.Add(m);
            }
        }
    }

    private async Task OpenOpenBinary()
    {
        OpenFileDialog openFileDialog = new();
        openFileDialog.Filter = "ProtoBuf Binary Files (*.pb)|*.pb";
        if (openFileDialog.ShowDialog() is true)
        {
            using var file = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
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
