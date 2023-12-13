using CommunityToolkit.Mvvm.Input;
using Google.Protobuf;
using Microsoft.Win32;
using ProtoBuf.Logic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using static ProtoBuf.Antlr.Protobuf3Parser;

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
            ParseResult = (ProtoContext?)ProtoParser.ParseFile(openFileDialog.FileName);
            var visitor = new MessageViewModel.Visitor();
            Messages.Clear();
            visitor.Visit(ParseResult);
            foreach (var m in visitor.Messages.Where(x => x.Parent == null))
            {
                Messages.Add(m);
            }
        }
    }

    private async Task OpenOpenBinary()
    {
        if (SelectedMessage == null || ParseResult == null)
        {
            return;
        }

        OpenFileDialog openFileDialog = new();
        openFileDialog.Filter = "ProtoBuf Binary Files (*.pb)|*.pb";
        if (openFileDialog.ShowDialog() is true)
        {
            using var file = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
            using var coded = CodedInputStream.CreateWithLimits(file, int.MaxValue, int.MaxValue);
            var decoder = new TypedMessageDecoder();
            var parseResult = decoder.Parse(coded, ParseResult, SelectedMessage.MessageDefContext);
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}