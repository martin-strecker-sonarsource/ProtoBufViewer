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
        ProtoFile = Settings.Default.ProtoFile;
        SelectedMessage = Messages.FirstOrDefault(x => x.Name == Settings.Default.SelectedMessage);
        if (SelectedMessage != null)
        {
            ProtoBinFile = Settings.Default.ProtoBinFile;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public AsyncRelayCommand OpenProtoCommand { get; }
    public AsyncRelayCommand OpenBinaryCommand { get; }

    public ObservableCollection<MessageViewModel> Messages { get; } = new();
    public MessageViewModel? SelectedMessage
    {
        get => selectedMessage;
        set
        {
            selectedMessage = value;
            OnPropertyChanged();
            OpenBinaryCommand.NotifyCanExecuteChanged();
            Settings.Default.SelectedMessage = value?.Name;
            Settings.Default.Save();
        }
    }

    protected ProtoContext? ParseResult { get => parseResult; set { parseResult = value; OnPropertyChanged(); } }


    private IReadOnlyCollection<TypedMessage>? typedMessages;
    public IReadOnlyCollection<TypedMessage>? TypedMessages { get => typedMessages; set { typedMessages = value; OnPropertyChanged(); } }
    public string ProtoFile
    {
        get => Settings.Default.ProtoFile;
        set
        {
            Settings.Default.ProtoFile = value;
            Settings.Default.Save();
            OnPropertyChanged();
            ParseProto();
        }
    }
    public string ProtoBinFile
    {
        get => Settings.Default.ProtoBinFile;
        set
        {
            Settings.Default.ProtoBinFile = value;
            Settings.Default.Save();
            OnPropertyChanged();
            ParseBinary();
        }
    }

    private async Task OpenProto()
    {
        OpenFileDialog openFileDialog = new();
        openFileDialog.Filter = "ProtoBuf Files (*.proto)|*.proto";
        if (openFileDialog.ShowDialog() is true)
        {
            ProtoFile = openFileDialog.FileName;
        }
    }

    private void ParseProto()
    {
        Messages.Clear();
        if (ProtoFile is { } file && File.Exists(file))
        {
            ParseResult = ProtoParser.ParseFile(ProtoFile);
            var visitor = new MessageViewModel.Visitor();
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
            ProtoBinFile = openFileDialog.FileName;
        }
    }

    private void ParseBinary()
    {
        TypedMessages = null;
        if (ProtoBinFile is { } f && File.Exists(f))
        {
            using var file = new FileStream(f, FileMode.Open, FileAccess.Read);
            using var coded = CodedInputStream.CreateWithLimits(file, int.MaxValue, int.MaxValue);
            var decoder = new TypedMessageDecoder();
            TypedMessages = decoder.Parse(coded, ParseResult, SelectedMessage.MessageDefContext);
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}