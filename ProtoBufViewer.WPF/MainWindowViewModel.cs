using CommunityToolkit.Mvvm.Input;
using Google.Protobuf;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using ProtoBuf.Logic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
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
        Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
        {
            ProtoFile = new(Settings.Default.ProtoFile ?? SpecialDirectories.MyDocuments);
            SelectedMessage = Messages.FirstOrDefault(x => x.Name == Settings.Default.SelectedMessage);
            if (SelectedMessage != null)
            {
                ProtoBinFile = new(Settings.Default.ProtoBinFile ?? SpecialDirectories.MyDocuments);
            }
        }));
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
    public FileInfo ProtoFile
    {
        get => new(Settings.Default.ProtoFile);
        set
        {
            Settings.Default.ProtoFile = value?.FullName;
            Settings.Default.Save();
            OnPropertyChanged();
            ParseProto();
        }
    }
    public FileInfo ProtoBinFile
    {
        get => new(Settings.Default.ProtoBinFile);
        set
        {
            Settings.Default.ProtoBinFile = value?.FullName;
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
            ProtoFile = new(openFileDialog.FileName);
        }
    }

    private void ParseProto()
    {
        try
        {
            Messages.Clear();
            if (ProtoFile is { Exists: true, FullName: { } file } )
            {
                ParseResult = ProtoParser.ParseFile(file);
                var visitor = new MessageViewModel.Visitor();
                visitor.Visit(ParseResult);
                foreach (var m in visitor.Messages.Where(x => x.Parent == null))
                {
                    Messages.Add(m);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
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
            ProtoBinFile = new(openFileDialog.FileName);
        }
    }

    private void ParseBinary()
    {
        TypedMessages = null;
        try
        {
            if (ProtoBinFile is { Exists: true, FullName: { } file })
            {
                using var fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                using var coded = CodedInputStream.CreateWithLimits(fs, int.MaxValue, int.MaxValue);
                var decoder = new TypedMessageDecoder();
                TypedMessages = decoder.Parse(coded, ParseResult, SelectedMessage.MessageDefContext);
            }
        } catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}